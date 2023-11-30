# advent-of-code-2023 ![Test Status](https://github.com/PaulWild/advent-of-code-2023/actions/workflows/dotnet.yml/badge.svg?branch=main)

Solutions to the Advent of code 2023 in C#

## Environment Variables

AOC_SESSION_ID: This is a session id taken from the advent of code website when logged in. Used by bootstrap to download the day's input.

## Bootstrap a day

To create some base files for a new day

`dotnet run --project adventofcode bootstrap x`

where x is the day to run

## Run solution for a day

`dotnet run --project adventofcode aoc x`

where x is the day to run

## Test

`dotnet test`

All commands should be run from the root directory
