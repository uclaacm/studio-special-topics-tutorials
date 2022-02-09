#include "Engine.h"

Engine::Engine() : windowIsClosed(false), hasPrintedLogs(false)
{
}

void Engine::Initialize()
{
}

UINT Engine::HandleMessage(MSG msg)
{
	if (msg.message == WM_QUIT)
	{
		return 1;
	}

	TranslateMessage(&msg);
	DispatchMessage(&msg);

	// Register player input
	//GameMain::Instance()->GetInput()->RegisterInput(&msg);

	return 0;
}

void Engine::Render()
{
	if (!hasPrintedLogs)
	{
		OutputDebugString(L"Draw something \n");
		hasPrintedLogs = true;
	}
}

WPARAM Engine::RunMainLoop()
{
	// while loop!!!!!
	MSG msg = { 0 };		// Struct to hold incoming windows message
	while (!windowIsClosed)
	{
		while (PeekMessage(&msg, nullptr, 0, 0, PM_REMOVE) > 0)
		{
			UINT result = HandleMessage(msg);
			if (result != 0)
			{
				return msg.wParam;
			}
			Render();
		}
	}

	return msg.wParam;
}
