var http = require('http');
var mjAPI = require('mathjax-node-svg2png');
const atob = require('atob');

mjAPI.start();

function base64ToUint8Array(base64String) {
    const padding = '='.repeat((4 - base64String.length % 4) % 4);
    const base64 = (base64String + padding)
        .replace(/\-/g, '+')
        .replace(/_/g, '/');

    const rawData = atob(base64);
    const outputArray = new Uint8Array(rawData.length);

    for (let i = 0; i < rawData.length; ++i) {
        outputArray[i] = rawData.charCodeAt(i);
    }
    return outputArray;
}

var server = http.createServer((req, res) => {
    try {
        if (req.url.indexOf("/?equation=") != -1) {
            let equation = req.url.split("/?equation=").filter(x => x);
            if (equation) {
                mjAPI.typeset({
                    math: equation[0],
                    format: "TeX", // or "inline-TeX", "MathML"
                    svg: false,      // or svg:true, or html:true,
                    png: true
                }).then(result => {
                    console.log(typeof (result.png));
                    res.write(base64ToUint8Array(result.png));
                    // res.write(new Buffer(result.png, 'base64'));
                    // res.write(`<img src='${result.svg}' />`);
                    res.end();
                }).catch(e => {
                    console.error(e);
                    res.statusCode = 500;
                    res.end();
                });
            }
        }
    }
    catch (e) {
        console.error(e);
        res.statusCode = 500;
        res.end();
    }
})


server.listen(5000, () => {
    console.log("started");
})


// a simple TeX-input example
// var mjAPI = require('mathjax-node-svg2png');

// var yourMath = 'E = mc^2';

// mjAPI.typeset({
//     math: yourMath,
//     format: "TeX", // or "inline-TeX", "MathML"
//     svg: false,
//     png: true,      // or svg:true, or html:true
// }, function (data) {
//     if (!data.errors) { console.log(data) }
// });