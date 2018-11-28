#!/usr/local/bin/node
const glob = require('glob');
const path = require('path');
const karma = require('karma');
const webpack = require('webpack');
const { spawn } = require('child_process');

// { production: true }

const webpackConfig = require('./webpack.config.js');

const compiler = webpack(webpackConfig);

compiler.watch({
    // Example watchOptions
    aggregateTimeout: 300,
    poll: undefined
}, (err, stats) => {
    if( err || stats.hasErrors() ) {
        console.log(err || stats.compilation.errors);
    } else {
        console.log("(web)Packed and all is good")
    }
});
