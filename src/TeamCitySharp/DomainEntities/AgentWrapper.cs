﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace TeamCitySharp.DomainEntities
{
  public class AgentWrapper
  {
    [JsonProperty("agent")]
    public List<Agent> Agent { get; set; }

    [JsonProperty("count")]
    public string Count { get; set; }

    [JsonProperty("href")]
    public string Href { get; set; }

    public override string ToString()
    {
      return "agents";
    }
  }
}