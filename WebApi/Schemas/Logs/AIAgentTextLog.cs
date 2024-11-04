﻿namespace WebApi.Schemas.Logs;

public class AIAgentTextLog(string fallbackText, DateTime timestamp) : Log("chat.aiagent.text", timestamp)
{
    public string fallback_text { get; private set; } = fallbackText;
    public AgentSender sender { get; private set; } = new AgentSender("BOT");
}