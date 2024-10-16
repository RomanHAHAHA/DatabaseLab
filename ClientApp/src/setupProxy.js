const { createProxyMiddleware } = require('http-proxy-middleware');
const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:4760';

const context =  [
  "/api/actors/get-all",
  "/api/actors/create",
  "/api/actors/delete",
  "/api/actors/update",
  "/api/actors/with-contract-data",
  "/api/actors/with-private-data",
  "/api/actors/add-to-agency",
  "/api/actors/with-max-contract-price",

  "/api/spectacles/update",
  "/api/spectacles/delete",
  "/api/spectacles/create",
  "/api/spectacles/get-all",
  "/api/spectacles/with-total-info",
  "/api/spectacles/with-actor-agency-name",

  "/api/contracts/update",
  "/api/contracts/delete",
  "/api/contracts/create",
  "/api/contracts/get-all",
  "/api/contracts/in-each-agency",

  "/api/actor-details/create",
  "/api/actor-details/update",
  "/api/actor-details/delete",
  "/api/actor-details/get-all",
  "/api/actor-details/by-agency",

  "/api/agencies/create",
  "/api/agencies/update",
  "/api/agencies/delete",
  "/api/agencies/get-all",
  "/api/agencies/with-actor-groups",

  "/api/reports/generate-report",
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
