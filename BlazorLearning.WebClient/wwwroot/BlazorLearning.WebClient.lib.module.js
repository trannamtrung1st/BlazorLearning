export function beforeStart(options, extensions) {
    console.log('BlazorLearning.WebClient.lib.module.js beforeStart');
}

export function afterStarted(blazor) {
    console.log('BlazorLearning.WebClient.lib.module.js afterStarted');
    blazor.registerCustomEventType('customevent', {
        createEventArgs: customEventArgsCreator
    });
    blazor.registerCustomEventType('customdblclick', {
        browserEventName: 'dblclick',
        createEventArgs: event => {
            return {
                author: 'TNT',
                elementHtml: event.target.outerHTML
            };
        }
    });
}

function customEventArgsCreator(event) {
    const customEvent = {
        ...event.detail,
        author: 'TNT'
    };
    return customEvent;
}