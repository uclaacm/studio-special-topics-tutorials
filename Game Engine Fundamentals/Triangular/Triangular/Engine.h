#pragma once

#include "windows.h"

class Engine
{
public:
	Engine();

	/// <summary>
	/// Explicitly initialize engine resources
	/// </summary>
	void Initialize();

	/// <summary>
	/// What en engine does
	/// </summary>
	WPARAM RunMainLoop();

private:
	UINT HandleMessage(MSG msg);
	void Render();

	bool windowIsClosed;
	bool hasPrintedLogs;
};
