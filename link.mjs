import { existsSync, mkdirSync, symlinkSync, unlinkSync } from "fs";
import { dirname, resolve } from "path";

function ensureDirSync(dirPath) {
  if (!existsSync(dirPath))
    mkdirSync(dirPath, { recursive: true });
}

function createSymlink(source, target) {
  if (existsSync(target))
    unlinkSync(target);

  symlinkSync(source, target, "junction");
}

function main() {
  const nodeModulesPath = resolve("node_modules");
  const targetLibPath = resolve("wwwroot", "lib");

  ensureDirSync(dirname(targetLibPath));
  createSymlink(nodeModulesPath, targetLibPath);
}

main();
