
(async function () {

	const executingScript = document.currentScript;
	if(!executingScript) {
		console.err("embedded.js MUST be run using a <script> tag in the current version.");
		return;
	}

	const executingScriptAbsolutePath = (new URL(executingScript.src, document.location)).href;

	const package = "package_ab0de366b29611d5afebd7dcd53b43110d6e3734";
	const absolutePath = (new URL(package, executingScriptAbsolutePath)).href;

	const styles = ["normalize.css","uno-bootstrap.css"];

	styles.forEach(s => {
		const scriptElement = document.createElement("link");
		scriptElement.setAttribute("href", `${absolutePath}/${s}`);
		scriptElement.setAttribute("rel", "stylesheet");
		document.head.appendChild(scriptElement);
	});

	document.baseURI = absolutePath;
    document.uno_app_base_override = absolutePath;
	const baseElement = document.createElement("base");
	baseElement.setAttribute("href", absolutePath);
	document.head.appendChild(baseElement);

	const html = "<div id='uno-body' class='container-fluid uno-body'><div class='uno-loader' loading-position='bottom' loading-alert='none'><img class='logo' src='' /><progress></progress><span class='alert'></span></div></div>";

	if(typeof unoRootElement !== 'undefined') {
		unoRootElement.innerHTML = html;
	} else {
		var rootDiv = document.createElement("div");
		rootDiv.innerHTML = html;
		document.body.appendChild(rootDiv);
	}

	const loadScript = s => new Promise((ok, err) => {
		const scriptElement = document.createElement("script");
		scriptElement.setAttribute("src", `${absolutePath}/${s}.js`);
		scriptElement.setAttribute("type", "text/javascript");
		scriptElement.onload = () => ok();
		scriptElement.onerror = () => err("err loading " + s);
		document.head.appendChild(scriptElement);
	});

	// Preload RequireJS dependency
	await loadScript("require");

	// Launch the bootstrapper
	await import(absolutePath + "/uno-bootstrap.js");

	// Yield to the browser to render the splash screen
	await new Promise(r => setTimeout(r, 0));

	// Dispatch the DOMContentLoaded event
    const loadedEvent = new Event("DOMContentLoaded");
	document.dispatchEvent(loadedEvent);
})();
