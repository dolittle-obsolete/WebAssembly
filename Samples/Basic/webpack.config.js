const config = require('@dolittle/build.aurelia/webpack.config.js')();
const CopyWebpackPlugin = require('copy-webpack-plugin');
const { ServiceWorkerGenerator } = require('@dolittle/webassembly.webpack');

config.entry = {
    app: ['@babel/polyfill', 'aurelia-bootstrapper'],
    vendor: ['bluebird']
};

config.plugins.push(new CopyWebpackPlugin([
    { from: 'publish/managed/**/*', to: 'managed', flatten: true },
    { from: 'publish/mono.js', to: 'mono.js', flatten: true },
    { from: 'publish/mono.wasm', to: 'mono.wasm', flatten: true },
    { from: 'manifest.json', to: 'manifest.json', flatten: true },
    { from: 'dolittle.png', to: 'dolittle.png', flatten: true }
]));

config.plugins.push(new ServiceWorkerGenerator());

module.exports = config;