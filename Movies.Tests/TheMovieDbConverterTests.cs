using Microsoft.Extensions.Options;
using Moq;
using Movies.Converters;
using Movies.Models.Configuration;
using Movies.Models.Responses;
using Movies.Models.Vendor.TheMovieDb;
using NUnit.Framework;
using System;

namespace Movies.Tests
{
    [TestFixture]
    public class TheMovieDbConverterTests
    {
        [Test]
        public void ConvertMovieListItemToQuickInfo_EmptyData_ReturnsNull()
        {
            var movieDbOptions = Mock.Of<IOptionsMonitor<TheMovieDbApiOptions>>();
            var converter = new TheMovieDbConverter(movieDbOptions);
            var result = converter.ConvertMovieListItemToQuickInfo(null);
            Assert.IsNull(result);
        }

        [Test]
        [TestCase(null, "http://nopath.com")]
        [TestCase("/poster.jpg", "http://path.com/poster.jpg")]
        public void ConvertMovieListItemToQuickInfo_Poster_ReturnsValidMovieObject(string poster, string expectedPoster)
        {
            TheMovieDbApiOptions options = new TheMovieDbApiOptions()
            {
                PosterPath = "http://path.com",
                NoPosterPath = "http://nopath.com"
            };
            var optionsMonitorMock = new Mock<IOptionsMonitor<TheMovieDbApiOptions>>();
            optionsMonitorMock.Setup(x => x.CurrentValue).Returns(options);
            var converter = new TheMovieDbConverter(optionsMonitorMock.Object);

            MovieGeneralInfo movieGeneralInfo = new MovieGeneralInfo()
            {
                Adult = false,
                BackdropPath = "/srYya1ZlI97Au4jUYAktDe3avyA.jpg",
                GenreIds = new int[] { 14, 28, 12 },
                Id = 464052,
                OriginalLanguage = "en",
                OriginalTitle = "Wonder Woman 1984",
                Overview = "Wonder Woman comes into conflict with the Soviet Union during the Cold War in the 1980s and finds a formidable foe by the name of the Cheetah.",
                Popularity = 4749.437,
                PosterPath = poster,
                ReleaseDate = new DateTime(2020, 12, 16),
                Title = "Wonder Woman 1984",
                Video = false,
                VoteAverage = 7.2,
                VoteCount = 2385
            };
            MovieQuickInfo movieQuickInfo = new MovieQuickInfo()
            {
                MovieId = 464052,
                IsFavorite = false,
                Title = "Wonder Woman 1984",
                ReleaseYear = 2020,
                Rank = 7.2,
                Poster = expectedPoster
            };
            var result = converter.ConvertMovieListItemToQuickInfo(movieGeneralInfo);
            Assert.AreEqual(movieQuickInfo.MovieId, result.MovieId);
            Assert.AreEqual(movieQuickInfo.Title, result.Title);
            Assert.AreEqual(movieQuickInfo.ReleaseYear, result.ReleaseYear);
            Assert.AreEqual(movieQuickInfo.Rank, result.Rank);
            Assert.AreEqual(movieQuickInfo.Poster, result.Poster);
        }

        [Test]
        public void ConvertMovieListItemToQuickInfo_NoReleaseDate_ReturnsValidMovieObject()
        {
            var optionsMonitorMock = new Mock<IOptionsMonitor<TheMovieDbApiOptions>>();
            optionsMonitorMock.Setup(x => x.CurrentValue).Returns(new TheMovieDbApiOptions());
            var converter = new TheMovieDbConverter(optionsMonitorMock.Object);

            MovieGeneralInfo movieGeneralInfo = new MovieGeneralInfo()
            {
                ReleaseDate = null
            };

            var result = converter.ConvertMovieListItemToQuickInfo(movieGeneralInfo);
            Assert.IsNull(result.ReleaseYear);
        }



        [Test]
        public void ConvertMovieWithVideosToMovieDetails_EmptyData_ReturnsNull()
        {
            var movieDbOptions = Mock.Of<IOptionsMonitor<TheMovieDbApiOptions>>();
            var converter = new TheMovieDbConverter(movieDbOptions);
            var result = converter.ConvertMovieWithVideosToMovieDetails(null);
            Assert.IsNull(result);
        }

        [Test]
        public void ConvertMovieWithVideosToMovieDetails_GeneralFields_ReturnsValidDetails()
        {
            TheMovieDbApiOptions options = new TheMovieDbApiOptions()
            {
                PosterPath = "http://path.com",
                NoPosterPath = "http://nopath.com"
            };
            var optionsMonitorMock = new Mock<IOptionsMonitor<TheMovieDbApiOptions>>();
            optionsMonitorMock.Setup(x => x.CurrentValue).Returns(options);
            var converter = new TheMovieDbConverter(optionsMonitorMock.Object);

            MovieWithVideosInfo movieWithVideosInfo = new MovieWithVideosInfo()
            {
                Adult = false,
                BackdropPath = "/srYya1ZlI97Au4jUYAktDe3avyA.jpg",
                GenreIds = new int[] { 14, 28, 12 },
                Id = 464052,
                OriginalLanguage = "en",
                OriginalTitle = "Wonder Woman 1984",
                Overview = "Wonder Woman comes into conflict with the Soviet Union during the Cold War in the 1980s and finds a formidable foe by the name of the Cheetah.",
                Popularity = 4749.437,
                ReleaseDate = new DateTime(2020, 12, 16),
                Title = "Wonder Woman 1984",
                Video = false,
                VoteAverage = 7.2,
                VoteCount = 2385,
                Budget = 200000000,
                Revenue = 131400000,
                Homepage = "https://www.warnerbros.com/movies/wonder-woman-1984",
                ImdbId = "tt7126948",
                Runtime = 151,
                Videos = null
            };
            MovieDetails movieDetails = new MovieDetails()
            {
                Budget = 200000000,
                Revenue = 131400000,
                Website = "https://www.warnerbros.com/movies/wonder-woman-1984",
                ImdbLink = "https://www.imdb.com/title/tt7126948",
                Duration = "02:31:00",
                Teaser = null
            };
            var result = converter.ConvertMovieWithVideosToMovieDetails(movieWithVideosInfo);
            Assert.AreEqual(movieDetails.Budget, result.Budget);
            Assert.AreEqual(movieDetails.Revenue, result.Revenue);
            Assert.AreEqual(movieDetails.Website, result.Website);
            Assert.AreEqual(movieDetails.ImdbLink, result.ImdbLink);
            Assert.AreEqual(movieDetails.Duration, result.Duration);
            Assert.AreEqual(movieDetails.Teaser, result.Teaser);
            Assert.IsNotNull(result.General);
        }

        [Test]
        public void ConvertMovieWithVideosToMovieDetails_WithYoutubeTeaser_ReturnsValidDetails()
        {
            TheMovieDbApiOptions options = new TheMovieDbApiOptions()
            {
                YoutubeEmbed = "<iframe src=\"youtube.com/{video-id}\"/>"
            };
            var optionsMonitorMock = new Mock<IOptionsMonitor<TheMovieDbApiOptions>>();
            optionsMonitorMock.Setup(x => x.CurrentValue).Returns(options);
            var converter = new TheMovieDbConverter(optionsMonitorMock.Object);

            MovieWithVideosInfo movieWithVideosInfo = new MovieWithVideosInfo()
            {
                Videos = new ListData<VideoInfo>()
                {
                    Result = new System.Collections.Generic.List<VideoInfo>()
                    {
                        new VideoInfo()
                        {
                            Site = "YouTube",
                            Key = "qwerty",
                            Type = "Teaser"
                        }
                    }
                }
            };
            MovieDetails movieDetails = new MovieDetails()
            {
                Teaser = "<iframe src=\"youtube.com/qwerty\"/>"
            };
            var result = converter.ConvertMovieWithVideosToMovieDetails(movieWithVideosInfo);

            Assert.AreEqual(movieDetails.Teaser, result.Teaser);
        }

        [Test]
        public void ConvertMovieWithVideosToMovieDetails_WithoutYoutubeTeaser_ReturnsValidDetails()
        {
            var optionsMonitorMock = new Mock<IOptionsMonitor<TheMovieDbApiOptions>>();
            optionsMonitorMock.Setup(x => x.CurrentValue).Returns(new TheMovieDbApiOptions());
            var converter = new TheMovieDbConverter(optionsMonitorMock.Object);

            MovieWithVideosInfo movieWithVideosInfo = new MovieWithVideosInfo()
            {
                Videos = new ListData<VideoInfo>()
                {
                    Result = new System.Collections.Generic.List<VideoInfo>()
                    {
                        new VideoInfo()
                        {
                            Site = "YouTube",
                            Key = "qwerty",
                            Type = "Trailer"
                        }
                    }
                }
            };
            var result = converter.ConvertMovieWithVideosToMovieDetails(movieWithVideosInfo);

            Assert.IsNull(result.Teaser);
        }
    }
}
