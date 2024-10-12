const { createProxyMiddleware } = require('http-proxy-middleware');
const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:4760';

const context =  [
  "/api/actors/get-all",
  "/api/actors/create",
  "/api/actors/delete",
  "/api/actors/update",
  "/api/actors/get-by-prefix",
  "/api/actors/get-by-experience",
  "/api/actors/get-by-rank",
  "/api/actors/with-contract-data",
  "/api/actors/with-private-data",

  "/api/spectacles/update",
  "/api/spectacles/delete",
  "/api/spectacles/create",
  "/api/spectacles/get-all",
  "/api/spectacles/with-budget",
  "/api/spectacles/with-production-year",
  "/api/spectacles/with-prefix",

  "/api/contracts/update",
  "/api/contracts/delete",
  "/api/contracts/create",
  "/api/contracts/get-all",
  "/api/contracts/with-price",
  "/api/contracts/with-role",
  "/api/contracts/of-author",
  "/api/contracts/of-spectacle",

  "/api/actor-details/create",
  "/api/actor-details/update",
  "/api/actor-details/delete",
  "/api/actor-details/get-all",
];

module.exports = function(app) {
  const appProxy = createProxyMiddleware(context, {
    proxyTimeout: 10000,
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  });

  app.use(appProxy);
};
