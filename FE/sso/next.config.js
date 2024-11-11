/** @type {import('next').NextConfig} */

const nextConfig = {
  rewrites: async () => [
    {
      source: "/api/:path*",
      destination: `${process.env.API_HOST}/:path*`,
    },
  ],
};

module.exports = nextConfig;
