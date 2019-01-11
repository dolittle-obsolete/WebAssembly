const config = require('@dolittle/build.aurelia/webpack.config.js')();
const CopyWebpackPlugin = require('copy-webpack-plugin');
const ServiceWorkerGenerator = require('@dolittle/webassembly.webpack/ServiceWorkerGenerator');

config.entry = {
    app: ['@babel/polyfill', 'aurelia-bootstrapper'],
    vendor: ['bluebird']
};

config.plugins.push(new CopyWebpackPlugin([
    { from: 'publish/managed/**/*', to: 'managed', flatten: true },
    { from: 'publish/mono.js', to: 'mono.js', flatten: true },
    { from: 'publish/mono.wasm', to: 'mono.wasm', flatten: true },
    { from: 'manifest.json', to: 'manifest.json', flatten: true },
    { from: 'dolittle.png', to: 'dolittle.png', flatten: true },
    { from: 'sqlite3.wasm', to: 'sqlite3.wasm', flatten: true },
    { from: 'sqlite3.js', to: 'sqlite3.js', flatten: true }
]));

config.plugins.push(new ServiceWorkerGenerator());

module.exports = config;