using Discord;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot
{
    public static class HtmlHelper
    {

        public static EmbedBuilder GetMetaFromUrl(string url)
        {
            // HTML AGILITY PACK
            HtmlWeb web = new HtmlWeb();
            web.PreRequest += (request) =>
            {
                request.Headers.Add("Accept-Language", "fr-FR");
                return true;
            }; // Setting HTTP Header to request fr-FR

            HtmlDocument doc = web.Load(url);
            var steamGame = new SteamGame();
            
            steamGame.Title = doc.DocumentNode
                               .SelectSingleNode("//title").InnerText;
            steamGame.Description = doc.DocumentNode.SelectSingleNode("//meta[@name='Description']").Attributes["content"].Value.ToString();
            //steamGame.PriceReal = doc.DocumentNode.SelectSingleNode("//meta[@itemprop='price']").Attributes["content"].Value.ToString();
            steamGame.ImageUrl = doc.DocumentNode.SelectSingleNode("//meta[@name='twitter:image']").Attributes["content"].Value.ToString();
            if (doc.DocumentNode.SelectNodes("//div[@class='game_area_purchase_game_wrapper']") != null)
            {
                steamGame.Games = new List<SteamGameArea>();
                var divgameArea = doc.DocumentNode.SelectNodes("//div[@class='game_area_purchase_game_wrapper']");
                foreach (var game in divgameArea)
                {
                    SteamGameArea gameArea = new SteamGameArea();

                    if (game.SelectSingleNode("//div[@class='discount_pct']") != null)
                        gameArea.HadReduction = true;
                    else 
                        gameArea.HadReduction = false;

                    if (game.SelectSingleNode("//div[@class='discount_pct']") != null)
                        gameArea.ReductionPercent = game.SelectSingleNode("//div[@class='discount_pct']").InnerText;
                    if (game.SelectSingleNode("//div[@class='discount_original_price']") != null)
                        gameArea.PriceReal = game.SelectSingleNode("//div[@class='discount_original_price']").InnerText;
                    if (game.SelectSingleNode("//div[@class='discount_final_price']") != null)
                        gameArea.PriceReduction = game.SelectSingleNode("//div[@class='discount_final_price']").InnerText;
                }
              
            }
            return FormatMetaToEmbed(steamGame);
        }

        public static EmbedBuilder FormatMetaToEmbed(SteamGame steamGame)
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithTitle($"{steamGame.Title}")
              .WithDescription($"{steamGame.Description}")
              .WithThumbnailUrl($"{steamGame.ImageUrl}");
            foreach (var game in steamGame.Games)
            {
                if (!game.HadReduction)
                    eb.AddField("**Prix **", $"{game.PriceReal}", true);
                else
                    eb.AddField("**Prix **", $"--{game.PriceReal}-- **{game.PriceReduction}**", true);
                eb.AddField("**Reduction**", $"*{game.ReductionPercent}*", true);
                
            }
            return eb;
        }
    }

    public class SteamGame
    {
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string HighlightScreenUrl { get; set; }

        public List<SteamGameArea> Games { get; set; }
    }

    public class SteamGameArea
    {
        public string PriceReal { get; set; }
        public string PriceReduction { get; set; }
        public bool HadReduction { get; set; }
        public string ReductionPercent { get; set; }
        public string Currency { get; set; }
    }
}
