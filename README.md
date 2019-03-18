# ![windows][windows] SendKeys
A tiny executable to send key input to any process by passing command line arguments to it. Use it to save yourself from adding refereces to `System.Windows.Forms`, for example in .NET Core or .NET Standard projects. 

SendKeys has no dependencies besides the .NET Framework 2.0 and can be deployed with any installer, for example. 

## Examples
###### Send keys to process 4711 and commit by sending an Enter key:
`SendKeys.exe -pid:4711 "format C:{Enter}"`

###### Same but wait 3 seconds upfront:
`SendKeys.exe -pid:4711 "format C:{Enter}" -wait:3000`

## Noteworthy

As always, you'll need to add quotes to the argument string if it contains spaces (like shown in the examples). Otherwise, Windows will split it up as multiple arguments.

[windows]: https://raw.githubusercontent.com/MarcBruins/awesome-xamarin/master/images/windows.png
