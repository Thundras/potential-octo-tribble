#!/usr/bin/env python3
from __future__ import annotations

import re
import sys
from datetime import date
from pathlib import Path


ASSEMBLY_PATH = Path("7D2D_ServerInfo/Properties/AssemblyInfo.cs")
RELEASE_NOTES_PATH = Path("docs/release-notes.md")
VERSION_PATTERN = re.compile(r'AssemblyVersion\(\"(\d+)\.(\d+)\.(\d+)\.(\d+)\"\)')


def read_version() -> tuple[int, int, int, int]:
    content = ASSEMBLY_PATH.read_text(encoding="utf-8")
    match = VERSION_PATTERN.search(content)
    if not match:
        raise SystemExit("AssemblyVersion not found.")
    return tuple(map(int, match.groups()))


def write_version(major: int, minor: int, patch: int) -> None:
    new_version = f"{major}.{minor}.{patch}.0"
    content = ASSEMBLY_PATH.read_text(encoding="utf-8")
    content = re.sub(
        r'AssemblyVersion\(\"[^\"]+\"\)',
        f'AssemblyVersion("{new_version}")',
        content,
    )
    content = re.sub(
        r'AssemblyFileVersion\(\"[^\"]+\"\)',
        f'AssemblyFileVersion("{new_version}")',
        content,
    )
    ASSEMBLY_PATH.write_text(content, encoding="utf-8")


def update_release_notes(major: int, minor: int, patch: int) -> None:
    notes = RELEASE_NOTES_PATH.read_text(encoding="utf-8")
    header = f"## {major}.{minor}.{patch}"
    if header in notes:
        return
    insertion = (
        f"{header} ({date.today().isoformat()})\n\n"
        "### Added\n"
        "- Automated release.\n\n"
    )
    updated = notes.replace("## Unreleased\n\n", "## Unreleased\n\n" + insertion)
    RELEASE_NOTES_PATH.write_text(updated, encoding="utf-8")


def bump_version(bump: str) -> str:
    major, minor, patch, _build = read_version()
    if bump == "major":
        major += 1
        minor = 0
        patch = 0
    elif bump == "minor":
        minor += 1
        patch = 0
    else:
        patch += 1

    write_version(major, minor, patch)
    update_release_notes(major, minor, patch)
    return f"{major}.{minor}.{patch}"


def main() -> None:
    if len(sys.argv) < 2:
        raise SystemExit("Usage: version_release.py <current|bump> [bump]")

    command = sys.argv[1]
    if command == "current":
        major, minor, patch, build = read_version()
        print(f"{major}.{minor}.{patch}.{build}")
        return
    if command == "bump":
        bump = sys.argv[2] if len(sys.argv) > 2 else "patch"
        print(bump_version(bump))
        return

    raise SystemExit("Usage: version_release.py <current|bump> [bump]")


if __name__ == "__main__":
    main()
