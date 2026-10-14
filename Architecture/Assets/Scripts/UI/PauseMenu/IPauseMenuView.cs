using System;

public interface IPauseMenuView
{
    event Action ResumeRequested;
    event Action MainMenuRequested;
    event Action SaveRequested;
    event Action LoadRequested;
}