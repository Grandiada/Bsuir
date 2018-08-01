
const ExtractTextPlugin = require("extract-text-webpack-plugin");
const webpack = require("webpack");

const extractSass = new ExtractTextPlugin({
  filename: "[name].style.css"
});

const path = require('path');

module.exports = {
  mode: 'development',//'production',
  entry: {
    index: './src/index.js',
  },
  devtool: 'source-map',
  module: {
    rules:
      [{
        test: /\.(jpe?g|png|gif)$/i,   //to support eg. background-image property 
        loader: "file-loader",
        query: {
          name: '[name].[ext]',
          outputPath: 'images/'
          //the images will be emmited to public/assets/images/ folder 
          //the images will be put in the DOM <style> tag as eg. background: url(assets/images/image.png); 
        }
      }, {
        test: /\.css$/,
        use: ['style-loader', 'css-loader']
      },
      {
        test: /\.tsx?$/,
        use: [
          {
            loader: 'ts-loader',
            options: {
              transpileOnly: true
            }
          }
        ]
      },
      {
        test: /\.scss$/,
        use: extractSass.extract({
          use: [{
            loader: "css-loader"
          }, {
            loader: "sass-loader"
          }],
          // use style-loader in development
          fallback: "style-loader"
        })
      }]
  },
  resolve: {
    extensions: ['.ts', '.tsx', ".js", ".json"]
  },
  output: {
    path: path.resolve(__dirname, '../../Bsuir.Web/wwwroot/dist'),
    filename: '[name].bundle.js'
  },
  plugins: [
    extractSass
  ]
};