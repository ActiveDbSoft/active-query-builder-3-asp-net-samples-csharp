module.exports = {
    entry: './index.js',
    output: {
        filename: 'index.js'
    },
    module: {
        loaders: [
            { test: /\.js$/, loader: 'babel-loader', exclude: /(node_modules|aqb.client.js)/ },
            { test: /\.jsx$/, loader: 'babel-loader', exclude: /node_modules/ }
        ]
    },
    devServer: {
        proxy: {
            '*': {
                target: 'http://localhost:1066'
            }
        }
    }
};