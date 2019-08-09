const { VueLoaderPlugin } = require("vue-loader");
const path = require("path");

module.exports = {
  mode: "development",
  entry: "./Client/main.js",
  output: {
    path: path.resolve(__dirname, "wwwroot"),
    filename: "main.js"
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
