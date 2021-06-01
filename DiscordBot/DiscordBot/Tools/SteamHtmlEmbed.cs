using Discord;
using Discord.Commands;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Tools
{
    public static class SteamHtmlEmbed
    {

        public static EmbedBuilder GetMetaFromUrl(string url, SocketCommandContext context)
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
            
            steamGame.Title = doc.DocumentNode.SelectSingleNode("//title").InnerText;
            steamGame.Description = doc.DocumentNode.SelectSingleNode("//meta[@name='Description']").Attributes["content"].Value.ToString();
            steamGame.ImageUrl = doc.DocumentNode.SelectSingleNode("//meta[@name='twitter:image']").Attributes["content"].Value.ToString();
            
            if (doc.DocumentNode.SelectNodes("//div[@class='game_area_purchase_game_wrapper']") != null)
            {
                steamGame.Games = new List<SteamGameArea>();
                var divgameArea = doc.DocumentNode.SelectNodes("//div[@class='game_area_purchase_game_wrapper']");
                foreach (var game in divgameArea)
                {
                    SteamGameArea gameArea = new SteamGameArea();
                    gameArea.HadReduction = false;
                    //Title of game area wrapper ( Game package ) 
                    if (game.SelectSingleNode(".//h1") != null)
                        gameArea.PackageName = game.SelectSingleNode(".//h1").InnerText;
                    //Looking if some reduction is apply for this one
                    if (game.SelectSingleNode(".//div[@class='discount_pct']") != null)
                    {
                        gameArea.ReductionPercent = game.SelectSingleNode(".//div[@class='discount_pct']").InnerText;
                        gameArea.HadReduction = true;
                    }
                    //Getting Price if reduction
                    if (game.SelectSingleNode(".//div[@class='discount_original_price']") != null)
                        gameArea.PriceReal = game.SelectSingleNode(".//div[@class='discount_original_price']").InnerText;
                    if (game.SelectSingleNode(".//div[@class='discount_final_price']") != null)
                        gameArea.PriceReduction = game.SelectSingleNode(".//div[@class='discount_final_price']").InnerText;
                    //TODO Update this methode to only use one of these conditions
                    //In this case we don't have any return from the div "game_purchase_price" we will use meta price to not send empty/null value
                    if (game.SelectSingleNode(".//div[@class='game_purchase_price']") != null)
                    {
                        gameArea.PriceReal = game.SelectSingleNode(".///div[@class='game_purchase_price']").InnerText;
                    }
                    if (String.IsNullOrEmpty(gameArea.PriceReal)) 
                    {
                        gameArea.PriceReal = doc.DocumentNode.SelectSingleNode("//meta[@itemprop='price']").Attributes["content"].Value;
                    } 


                    steamGame.Games.Add(gameArea);
                }
              
            }
            return FormatMetaToEmbed(steamGame, context, url) ;
        }

        public static EmbedBuilder FormatMetaToEmbed(SteamGame steamGame, SocketCommandContext context, string url)
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithTitle($"{steamGame.Title}")
              .WithDescription($"{steamGame.Description}")
              .WithImageUrl($"{steamGame.ImageUrl}")
              .WithUrl(url)
              .WithAuthor(context.Client.CurrentUser)
              .WithColor(Color.DarkBlue);
            foreach (var game in steamGame.Games)
              {
                eb.AddField($"{game.PackageName}", "\u200b");
                if (!game.HadReduction)
                {

                    eb.AddField("**Prix **", $"{game.PriceReal}", true);
                }
                else
                {
                    eb.AddField("**Prix **", $"> ~~ {game.PriceReal} **{game.PriceReduction}**~~", true);
                    eb.AddField("**Reduction**", $" > {game.ReductionPercent} ", true);
                    eb.AddField("\u200b", "\u200b", false);
                }

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
        public string PackageName { get; set; }
        public string PriceReal { get; set; }
        public string PriceReduction { get; set; }
        public bool HadReduction { get; set; }
        public string ReductionPercent { get; set; }
        public string Currency { get; set; }
    }
}
