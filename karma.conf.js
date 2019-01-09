'use strict';
const base = require('@dolittle/build/karma.conf.js');
module.exports = (config) => {
    base(config);
    config.set({
        basePath: '.',
    });
};
