const { VueLoaderPlugin } = require("vue-loader");
const UglifyJsPlugin = require("uglifyjs-webpack-plugin");
const path = require("path");

module.exports = {
  entry: "./Client/main.js",
  output: {
    path: path.resolve(__dirname, "wwwroot"),
    filename: "main.js"
  },
  optimization: {
    minimizer: [new UglifyJsPlugin()]
  },
  devServer: {
    contentBase: path.resolve(__dirname, "wwwroot")
  },
  module: {
    rules: [
      {
        test: /\.vue$/,
        use: "vue-loader"
      },
      {
        test: /\.s?css$/,
        use: ["vue-style-loader", "css-loader", "sass-loader"]
      }
    ]
  },
  plugins: [new VueLoaderPlugin()]
};
