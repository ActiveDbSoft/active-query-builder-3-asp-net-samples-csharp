module.exports = {
    entry: './index.js',
    output: {
        filename: 'index.js'
    },
    devServer: {
        proxy: {
            '*': {
                target: 'http://localhost:1066'
            }
        }
    }
};