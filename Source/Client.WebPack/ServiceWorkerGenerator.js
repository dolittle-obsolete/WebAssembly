/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
const fs = require('fs');
const path = require('path');
const serviceWorkerFile = 'service-worker.js';
const templateFile = path.join(__dirname, serviceWorkerFile);

function getSortedFiles(output, items, fileStartsWith) {
    let filteredSorted = items
        .filter(_ => _.indexOf(fileStartsWith) == 0)
        .sort((a, b) => fs.statSync(path.join(output, a)).mtimeMs < fs.statSync(path.join(output, b)).mtimeMs);

    return filteredSorted;
}

export class ServiceWorkerGenerator {
    apply(compiler) {
        compiler.hooks.afterEmit.tap('ServiceWorkerGenerator', compilation => {
            let root = process.cwd();
            let output = path.join(root, 'wwwroot');
            let assembliesFile = path.join(root, 'publish', 'assemblies.json');
            let outputFile = path.join(output, serviceWorkerFile);

            fs.readFile(templateFile, (err, content) => {
                let assets = [];
                let jsFile = content.toString();
                fs.readdir(output, (err, items) => {

                    let appFiles = getSortedFiles(output, items, 'app');
                    let vendorFiles = getSortedFiles(output, items, 'vendor');

                    if (appFiles.length > 0) assets.push(`/${appFiles[0]}`);
                    if (vendorFiles.length > 0) assets.push(`/${vendorFiles[0]}`);

                    items = items.filter(_ => _.indexOf('app') < 0);
                    items = items.filter(_ => _.indexOf('vendor') < 0);
                    items.forEach(item => assets.push(`/${item}`));

                    fs.readFile(assembliesFile, (err, assembliesAsJson) => {
                        assembliesAsJson = assembliesAsJson.toString()
                        var assemblies = JSON.parse(assembliesAsJson);
                        assemblies.forEach(assembly => assets.push(`/managed/${assembly}`));

                        let assetsCode = `var assets = ['${assets.join("','")}'];`;
                        let newFile = `${assetsCode}\n${jsFile}`;
                        fs.writeFileSync(outputFile, newFile);
                    });
                });
            });
        });
    }
}