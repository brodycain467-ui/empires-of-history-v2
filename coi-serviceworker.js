self.addEventListener("install",()=>self.skipWaiting());
self.addEventListener("activate",e=>e.waitUntil(self.clients.claim()));
function setCOI(r){const h=new Headers(r.headers);h.set("Cross-Origin-Embedder-Policy","require-corp");h.set("Cross-Origin-Opener-Policy","same-origin");return new Response(r.body,{status:r.status,statusText:r.statusText,headers:h})}
self.addEventListener("fetch",e=>{if(e.request.cache==="only-if-cached"&&e.request.mode!=="same-origin")return;e.respondWith(fetch(e.request).then(r=>setCOI(r)))});
