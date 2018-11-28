const config = require('@dolittle/build.aurelia/webpack.config.js')();
const CopyWebpackPlugin = require('copy-webpack-plugin');

config.entry = {
    app: ['babel-polyfill', 'aurelia-bootstrapper'],
    vendor: ['bluebird']
};

config.plugins.push(new CopyWebpackPlugin([
    { from: 'publish/managed/**/*', to: 'managed', flatten: true },
    { from: 'publish/mono.js', to: 'mono.js', flatten: true },
    { from: 'publish/mono-config.js', to: 'mono-config.js', flatten: true },
    { from: 'publish/runtime.js', to: 'runtime.js', flatten: true },
    { from: 'publish/mono.wasm', to: 'mono.wasm', flatten: true }
]));

module.exports = config;