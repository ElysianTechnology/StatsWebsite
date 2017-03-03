var webpack = require('webpack');
var path = require('path');
var config = require('./webpack.config');
var CompressionPlugin = require("compression-webpack-plugin");

config.output = {
  filename: '[name].bundle.js',
  publicPath: '',
  path: path.resolve(__dirname, 'dist')
};

config.plugins = config.plugins.concat([

  // Reduces bundles total size
  new webpack.optimize.UglifyJsPlugin({
    mangle: {

      // You can specify all variables that should not be mangled.
      // For example if your vendor dependency doesn't use modules
      // and relies on global variables. Most of angular modules relies on
      // angular global variable, so we should keep it unchanged
      except: ['$super', '$', 'exports', 'require', 'angular']
    },
    comments: false,
    beautify: false,
    compress: {
      screw_ie8: true,
      drop_console: true,
      dead_code: true
    }
  })
  // ,
  // new CompressionPlugin({
  //   asset: "[path].gz[query]",
  //   algorithm: "gzip",
  //   test: /\.js$|\.html$/,
  //   threshold: 0,
  //   minRatio: 0.8
  // })
]);

module.exports = config;