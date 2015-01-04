using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Filmtipset.API
{
        public enum FilmtipsetAPIReturntype
        {
            json,
            xml
        }

        public enum GradeType
        {
            seen,
            calculated,
            official,
            gubbe,
            none
        }

        public enum FilmtipsetAPIAction
        {
            recommendations,
            list,
            lists, //Custom action. Lists scraped from web page
            movie,
            user
        }

        public enum FimtipsetApiGrade
        {
            all = 0,
            one = 1,
            two = 2,
            three = 3,
            four = 4,
            five = 5
        }

        public enum FilmtipsetAPIListType
        {
            bio = 1,
            tv = 2,
            video = 3,
            owned = 4,
            wantedlist = 5,
            seen = 6,
            grades = 7
        }

        public enum FilmtipsetAPIGenre
        {
            GenerellaTips = 0,
            Drama = 1,
            Kortfilm = 2,
            Komedi = 3,
            Dokumentar = 4,
            Animerad = 5,
            Vuxenfilm = 6,
            Familjefilm = 7,
            Action = 8,
            Kriminalare = 9,
            Romantik = 10,
            Thriller = 11,
            Musikal = 12,
            Aventyr = 13,
            Western = 14,
            Skrack = 15,
            ScienceFiction = 16,
            Fantasy = 17,
            Mysterium = 18,
            Krig = 19,
            FilmNoir = 20,
            Scenshow = 21,
            Anime = 22,
            MiniSerie = 23,
            Stumfilm = 24,
            Amatorfilm = 25,
            Experimentfilm = 38,
            Roadmovie = 39,
            Biografi = 40
        }
}
