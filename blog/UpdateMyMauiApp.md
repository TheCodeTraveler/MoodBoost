## Hey Q! Add A New Feature to My Mobile App

Welcome to the **Hey Q!** blog series where we build fun apps using [Amazon Q Developer](https://docs.aws.amazon.com/amazonq/latest/qdeveloper-ug/command-line.html?trk=26a307dd-f6c6-4133-a99f-0388d1304aef&sc_channel=el) and learn about the code along the way! The scope of the sample apps created in this series are small 1-2 page applications that are fun, well designed and properly architected following best practices. And, the best part, the code is all open-source allowing you to grab it and use it for your apps! Here's the link to the completed open-source project: https://github.com/TheCodeTraveler/MoodBoost

This week we are updating an existing mobile app built in .NET MAUI. For this app, I have a few goals:
1. The app must compile and run on Windows Desktop
2. The app must have a nice, highly polished, professional User Interface (UI)
3. The app must have a fun name that relates to its use
4. The code must be created by first using the latest version of the **WPF** template in the latest version of Visual Studio
5. The code must use the MVVM Architecture
6. The code must follow best practices
7. The code must use the most-recent versions for all dependencies

## The Completed App

**Note:** If you're interested in learning more about prompt engineering and want to see the steps I used to guide Q Developer CLI, scroll down to the Appendix.



## Appendix

For anyone out there who is interested in learning more about prompt engineering, this section contains the steps I followed to create this app.

**Note:** Responses from LLMs are non-deterministic which can result in different outputs despite asking the same question. You may receive slightly different responses from Q Developer when trying to replicate the same steps below.

I've [already installed Q Developer CLI](https://docs.aws.amazon.com/amazonq/latest/qdeveloper-ug/command-line-installing.html?trk=26a307dd-f6c6-4133-a99f-0388d1304aef&sc_channel=el) on my Mac, so let's begin by opening the [macOS Terminal](https://support.apple.com/guide/terminal/welcome/mac) and begin chatting with Q using the following command:

```
> q chat
```

Next, I will help Q out a bit by giving Q a profile. This reduces the scope of the datasets that Q will reference helps Q better understand the problem we are trying to solve together:

**Prompt**
> Hey Q! You are a senior. NET MAUI Developer who is an expert in C# and always follows C#, Mobile and .NET MAUI best practices. You always reference the official C# documentation on https://learn.microsoft.com/dotnet/csharp/ when writing your code and you always reference the official .NET MAUI documentation when writing your code https://learn.microsoft.com/dotnet/maui/. You always write code the uses the recommended libraries `CommunityToolkit.MVVM` and `CommunityToolkit.Maui`. The User Interface for your apps are always highly polished and professional, giving the appearance that it was created by a Senior Designer. In addition to the reference domains above, you are also allowed to reference the open-source NET runtime code on GitHub, https://github.com/dotnet/runtime, and the open-source .NET MAUI code on GitHub https://github.com/dotnet/maui.