module.exports = {
    entry: './index.js',
    output: {
        filename: 'index.js'
    },
    devServer: {
        proxy: {
            '*': {
				"target": "https://[::1]:44384",
				"secure": false
            }			
        },
		'https': true
    }
};