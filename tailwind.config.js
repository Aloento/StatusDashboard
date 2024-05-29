import { Config } from "tailwindcss";
import { isolateOutsideOfContainer, scopedPreflightStyles } from "tailwindcss-scoped-preflight";

/** @type {Config} */
const config = {
  content: ["./**/*.razor"],
  theme: {
    extend: {},
  },
  plugins: [scopedPreflightStyles({
    isolationStrategy: isolateOutsideOfContainer([
      '[class^="fluent-"]',
      '[class^="scale-"]',
    ])
  })],
};

export default config;
