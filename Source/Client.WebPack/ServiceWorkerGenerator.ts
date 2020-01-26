// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import * as fs from 'fs';
import * as path from 'path';
const serviceWorkerFile = 'service-worker.js';
const templateFile = path.join(__dirname, serviceWorkerFile);

function getSortedFiles(output: string, items: string[], fileStartsWith: string) {
    const filteredSorted = items
        .filter(_ => _.indexOf(fileStartsWith) === 0)
        .sort((a, b) => fs.statSync(path.join(output, a)).mtimeMs - fs.statSync(path.join(output, b)).mtimeMs);

    return filteredSorted;
}

export class ServiceWorkerGenerator {
    constructor(private _config: any) {
        this._config = {
            assembliesFileFolder: path.join(process.cwd(),'publish'),
            outputFolder: path.join(process.cwd(),'wwwroot')
        };

        if (_config) {
            this._config.assembliesFileFolder = _config.assembliesFileFolder || this._config.assembliesFileFolder;
            this._config.outputFolder = _config.outputFolder || this._config.outputFolder;
        }
    }

    apply(compiler: any) {
        compiler.hooks.afterEmit.tap('ServiceWorkerGenerator', (compilation: any) => {
            const output = this._config.outputFolder;
            const assembliesFile = path.join(this._config.assembliesFileFolder, 'assemblies.json');
            const outputFile = path.join(output, serviceWorkerFile);

            fs.readFile(templateFile, (err: any, content: any) => {
                const assets: string[] = [];
                const jsFile = content.toString();

                fs.readdir(output, (err: any, items: string[]) => {

                    const appFiles = getSortedFiles(output, items, 'app');
                    const vendorFiles = getSortedFiles(output, items, 'vendor');

                    if (appFiles.length > 0) assets.push(`/${appFiles[0]}`);
                    if (vendorFiles.length > 0) assets.push(`/${vendorFiles[0]}`);

                    items = items.filter(_ => _.indexOf('app') < 0);
                    items = items.filter(_ => _.indexOf('vendor') < 0);
                    items.forEach(item => assets.push(`/${item}`));

                    fs.readFile(assembliesFile, (err, assembliesAsJsonBytes) => {
                        const assembliesAsJson = assembliesAsJsonBytes.toString();
                        const assemblies = JSON.parse(assembliesAsJson);
                        assemblies.forEach((assembly: string) => assets.push(`/managed/${assembly}`));

                        const assetsCode = `var assets = ['${assets.join("','")}'];`;
                        const newFile = `${assetsCode}\n${jsFile}`;
                        fs.writeFileSync(outputFile, newFile);
                    });
                });
            });
        });
    }
}
