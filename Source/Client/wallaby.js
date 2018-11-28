const wallaby = require('@dolittle/build/wallaby')
module.exports = wallaby('Source', null, (config) => {
    config.files.push({ pattern: `Source/**/node_modules/**/*.js`, ignore: true });
});
