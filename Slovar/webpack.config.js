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
    minimizer: [
      new UglifyJsPlugin({
        uglifyOptions: {
          output: {
            comments: false
          }
        }
      })
    ]
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
      },
      {
        test: /\.js/,
        exclude: /node_modules/,
        use: {
          loader: "babel-loader",
          options: {
            presets: ["@babel/preset-env", "babel-preset-minify"],
            plugins: ["@babel/transform-runtime"]
          }
        }
      }
    ]
  },
  plugins: [new VueLoaderPlugin()]
};
