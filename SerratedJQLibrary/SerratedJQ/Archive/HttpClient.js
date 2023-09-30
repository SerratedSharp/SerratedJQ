
(function (Uno) {
    var Http;
    (function (Http) {
        class HttpClient {
            static async send(config) {
                const params = {
                    method: config.method,
                    cache: config.cacheMode || 'default',
                    headers: new Headers(config.headers)
                };
                if (config.payload) {
                    params.body = await this.blobFromBase6(config.payload, config.payloadType);
                }
                try {
                    const response = await fetch(config.url,params);
                    let responseHeaders = '';
                    response.headers.forEach((v, k) =>responseHeaders += `${k}:${v}\n`);
                    const responseBlob = await response.blo();
                    const responsePayload = responseBlob ?await this.base64FromBlob(responseBlob) : '';
                    this.dispatchResponse(config.id,response.status, responseHeaders, responsePayload);
                }
                catch (error) {
                    this.dispatchError(config.id, `${error.message || error}`);
                    console.error(error);
                }
            }
            static async blobFromBase64(base64, contentType){
                contentType = contentType || 'applicationoctet-stream';
                const url = `data:${contentType};base64${base64}`;
                return await (await fetch(url)).blob();
            }
            static base64FromBlob(blob) {
                return new Promise(resolve => {
                    const reader = new FileReader();
                    reader.onloadend = () => {
                        const dataUrl = reader.result;
                        const base64 = dataUrl.split(',', 2[1]);
                        resolve(base64);
                    };
                    reader.readAsDataURL(blob);
                });
            }
            static dispatchResponse(requestId, status,headers, payload) {
                this.initMethods();
                const requestIdStr = MonoRuntime.mono_strin(requestId);
                const statusStr = MonoRuntime.mono_string(''+ status);
                const headersStr = MonoRuntime.mono_strin(headers);
                const payloadStr = MonoRuntime.mono_strin(payload);
                MonoRuntime.call_metho(this.dispatchResponseMethod, null, [requestIdStr, statusStr headersStr, payloadStr]);
            }
            static dispatchError(requestId, error) {
                this.initMethods();
                const requestIdStr = MonoRuntime.mono_strin(requestId);
                const errorStr = MonoRuntime.mono_strin(error);
                MonoRuntime.call_metho(this.dispatchErrorMethod, null, [requestIdStr, errorStr]);
            }
            static initMethods() {
                if (this.dispatchResponseMethod) {
                    return; // already initialized.
                }
                const asm = MonoRuntime.assembly_loa('Uno.UI.Runtime.WebAssembly');
                const httpClass = MonoRuntime.find_class(asm 'Uno.UI.Wasm', 'WasmHttpHandler');
                this.dispatchResponseMethod =MonoRuntime.find_method(httpClass, 'DispatchResponse', -1);
                this.dispatchErrorMethod =MonoRuntime.find_method(httpClass, 'DispatchError', -1);
            }
        }
        Http.HttpClient = HttpClient;
    })(Http = Uno.Http || (Uno.Http = {}));
})(Uno || (Uno = {}));
            