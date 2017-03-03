var path    = require('path');
var webpack = require('webpack');
var HtmlWebpackPlugin = require('html-webpack-plugin');

module.exports = {
  devtool: 'source-map',
  entry: {},
  module: {
    loaders: [
       { test: /\.js$/, exclude: [/app\/lib/, /node_modules/], use: 'babel-loader' },
       { test: /\.html$/, use: 'raw-loader' },
       { test: /\.scss$/, use: ['style-loader', 'css-loader', 'sass-loader?']},
       { test: /\.css$/, use: ['style-loader', 'css-loader'] },
       { test: /\.json/, use: 'json-loader' },
       { test: /\.ico$/, loader: 'url-loader?mimetype=image/ico'},
       { test: /\.(jpg|png|gif)?(\?v=[0-9]\.[0-9]\.[0-9])?$/, use: 'file-loader?name=img/[name].[ext]'},
       { test: /\.woff(2)?(\?v=[0-9]\.[0-9]\.[0-9])?$/, use: "url-loader?name=fonts/[name].[ext]&limit=10000&minetype=application/font-woff"},
       { test: /\.(ttf|eot|svg)?(\?v=[0-9]\.[0-9]\.[0-9])?$/, use: "file-loader?name=assets/fonts/[name].[ext]"},
    ]
  },
  // ,
  // node: {
  //   fs: "empty"
  // },
  plugins: [
    // Injects bundles in your index.html instead of wiring all manually.
    // It also adds hash to all injected assets so we don't have problems
    // with cache purging during deployment.
    new HtmlWebpackPlugin({
      template: 'app/index.html',
      inject: 'body',
      hash: true
    }),

    // Automatically move all modules defined outside of application directory to vendor bundle.
    // If you are using more complicated project structure, consider to specify common chunks manually.
    new webpack.optimize.CommonsChunkPlugin({
      name: 'vendor',
      minChunks: function (module, count) {
        return module.resource && module.resource.indexOf(path.resolve(__dirname, 'app')) === -1;
      }
    })
  ]
};
