var express = require('express');
var router = express.Router();
var mjAPI = require('mathjax-node');

mjAPI.start();

router.post("/", (req, res, next) => {
    try {
        console.log(req.body);
        if (req.body.equation) {
            try {
                mjAPI.typeset({
                    ex: 30,
                    width: 800,
                    math: req.body.equation,
                    format: "TeX", // or "inline-TeX", "MathML"
                    svg: true // or svg:true, or html:true,
                }).then(result => {
                    res.write(result.svg);
                    res.end();
                }).catch(e => {
                    console.error(e);
                    res.statusCode = 500;
                    res.end();
                });
            } catch {
                res.statusCode = 500;
                res.end();
            }
        } else {
            res.statusCode = 404;
            res.end();
        }
    } catch {
        res.statusCode = 500;
        res.end();
    }
})
router.post('/setstore', (req, resp, next) => {

})
module.exports = router;