// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { PLATFORM } from 'aurelia-pal';

export class app {
    constructor() {
    }

    router: any;

    configureRouter(config: any, router: any) {
        config.options.pushState = true;
        config.map([
            { route: [''], name: 'Home', moduleId: PLATFORM.moduleName('index') },
        ]);

        this.router = router;
    }
}
