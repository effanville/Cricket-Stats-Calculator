﻿using System.IO;

using Common.Structure.ReportWriting;

using CricketStructures.Match;


using NUnit.Framework;

namespace CricketStructures.Tests
{
    internal class FriendlyStringTests
    {
        [TestCase("HighestODIChase", Common.Structure.ReportWriting.DocumentType.Html)]
        [TestCase("HighestODIChase", Common.Structure.ReportWriting.DocumentType.Md)]
        public void DoStuff(string index, Common.Structure.ReportWriting.DocumentType exportType)
        {
            var matchToTest = TestCaseInstances.ExampleMatches[index];
            var friendlyString = matchToTest.SerializeToString(exportType);
            var stringThing = friendlyString.ToString();
            File.WriteAllText($"c:\\data\\source\\test{index}.{exportType.ToString()}", stringThing);
        }

        const string HtmlScorecard = @"<!DOCTYPE html>
<html lang=""en"">
<head>
<meta charset=""utf-8"" http-equiv=""x-ua-compatible"" content=""IE=11""/>
<title></title>
<style>
html, h1, h2, h3, h4, h5, h6 { font-family: ""Arial"", cursive, sans-serif; }
h1 { font-family: ""Arial"", cursive, sans-serif; margin-top: 1.5em; }
h2 { font-family: ""Arial"", cursive, sans-serif; margin-top: 1.5em; }
body{ font-family: ""Arial"", cursive, sans-serif; font-size: 10px; }
table { border-collapse: collapse; }
table { border: 1px solid black; }
th, td { border: 1px solid black; max-width: 175px; min-width: 25px;}
caption { margin-bottom: 1.2em; font-family: ""Arial"", cursive, sans-serif; font-size:medium; }
tr { text-align: center; }
div { max-width: 1000px; max-height: 600px; margin: left; margin-bottom: 1.5em; }
tr:nth-child(even) {background-color: #f0f8ff;}
th{ background-color: #ADD8E6; height: 1.5em; }
[data-negative] { background-color: red;}
p { line-height: 1.5em; margin-bottom: 1.5em;}
</style>
<script src=""https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.9.4/Chart.min.js""></script>
<script src=""https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js""></script>
<script src=""https://cdn.jsdelivr.net/npm/chart.js@2.9.4/dist/Chart.min.js""></script>
</head>
<body>
<h1>South Africa vs Australia. Venue: Joburg. Date: 12/03/2006 00:00:00. Type of Match: League</h1>
<h2>Innings of: Australia.</h2>
<h3>Batting</h3>
<table>
<thead><tr>
<th scope=""col""></th><th>Batsman</th><th>How Out</th><th>Bowler</th><th>Total</th>
</tr></thead>
<tbody>
<tr>
<td>1</td><td>AC Gilchrist</td><td>Caught AJ Hall</td><td>R Telemachus</td><td>55</td>
</tr>
<tr>
<td>2</td><td>SM Katich</td><td>Caught R Telemachus</td><td>M Ntini</td><td>79</td>
</tr>
<tr>
<td>3</td><td>RT Ponting</td><td>Caught HH Dippenaar</td><td>R Telemachus</td><td>164</td>
</tr>
<tr>
<td>4</td><td>MEK Hussey</td><td>Caught M Ntini</td><td>AJ Hall</td><td>81</td>
</tr>
<tr>
<td>5</td><td>A Symonds</td><td>NotOut  </td><td> </td><td>27</td>
</tr>
<tr>
<td>6</td><td>B Lee</td><td>NotOut  </td><td> </td><td>9</td>
</tr>
<tr>
<td>7</td><td>DR Martyn</td><td>DidNotBat  </td><td> </td><td>0</td>
</tr>
<tr>
<td>8</td><td>MJ Clarke</td><td>DidNotBat  </td><td> </td><td>0</td>
</tr>
<tr>
<td>9</td><td>NW Bracken</td><td>DidNotBat  </td><td> </td><td>0</td>
</tr>
<tr>
<td>10</td><td>SR Clark</td><td>DidNotBat  </td><td> </td><td>0</td>
</tr>
<tr>
<td>11</td><td>ML Lewis</td><td>DidNotBat  </td><td> </td><td>0</td>
</tr>
<tr>
<td></td><td></td><td></td><td>Batting Total</td><td>415</td>
</tr>
<tr>
<td></td><td></td><td></td><td>Total Extras </td><td>19</td>
</tr>
<tr>
<td></td><td></td><td></td><td>Total</td><td>434</td>
</tr>
</tbody>
</table>
<h3>Extras</h3>
<table>
<thead><tr>
<th scope=""col""></th><th></th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Byes</th><td>0</td>
</tr>
<tr>
<th scope=""row"">Leg Byes</th><td>4</td>
</tr>
<tr>
<th scope=""row"">Wides</th><td>5</td>
</tr>
<tr>
<th scope=""row"">No Balls</th><td>10</td>
</tr>
<tr>
<th scope=""row"">Penalties</th><td>0</td>
</tr>
<tr>
<th scope=""row"">Total Extras</th><td>19</td>
</tr>
</tbody>
</table>
<h3>Partnerships</h3>
<table>
<thead><tr>
<th scope=""col"">Wicket</th><th>FallOfWicket</th><th>ManOut</th>
</tr></thead>
<tbody>
<tr>
<td>1</td><td>97</td><td>1</td>
</tr>
<tr>
<td>2</td><td>216</td><td>2</td>
</tr>
<tr>
<td>3</td><td>374</td><td>4</td>
</tr>
<tr>
<td>4</td><td>407</td><td>3</td>
</tr>
</tbody>
</table>
<h3>Bowling</h3>
<table>
<thead><tr>
<th scope=""col"">Bowler</th><th>Wides</th><th>NB</th><th>Overs</th><th>Mdns</th><th>Runs</th><th>Wkts</th><th>Avg</th>
</tr></thead>
<tbody>
<tr>
<td>M Ntini</td><td>1</td><td>0</td><td>9</td><td>0</td><td>80</td><td>1</td><td>80</td>
</tr>
<tr>
<td>AJ Hall</td><td>0</td><td>2</td><td>10</td><td>0</td><td>80</td><td>1</td><td>80</td>
</tr>
<tr>
<td>JJ van der Wath</td><td>1</td><td>1</td><td>10</td><td>0</td><td>76</td><td>0</td><td>∞</td>
</tr>
<tr>
<td>R Telemachus</td><td>3</td><td>7</td><td>10</td><td>1</td><td>87</td><td>2</td><td>43.5</td>
</tr>
<tr>
<td>GC Smith</td><td>0</td><td>0</td><td>4</td><td>0</td><td>29</td><td>0</td><td>∞</td>
</tr>
<tr>
<td>JH Kallis</td><td>0</td><td>0</td><td>6</td><td>0</td><td>70</td><td>0</td><td>∞</td>
</tr>
<tr>
<td>JM Kemp</td><td>0</td><td>0</td><td>1</td><td>0</td><td>8</td><td>0</td><td>∞</td>
</tr>
<tr>
<td>Bowling Totals</td><td>5</td><td>10</td><td>50</td><td>1</td><td>430</td><td>4</td><td>107.5</td>
</tr>
</tbody>
</table>
<h3>Score</h3>
<p>Final Score: 434 for 4</p>

<h2>Innings of: South Africa.</h2>
<h3>Batting</h3>
<table>
<thead><tr>
<th scope=""col""></th><th>Batsman</th><th>How Out</th><th>Bowler</th><th>Total</th>
</tr></thead>
<tbody>
<tr>
<td>1</td><td>GC Smith</td><td>Caught MEK Hussey</td><td>SR Clarke</td><td>90</td>
</tr>
<tr>
<td>2</td><td>HH Dippenaar</td><td>Bowled  </td><td>NW Bracken</td><td>1</td>
</tr>
<tr>
<td>3</td><td>HH Gibbs</td><td>Caught B Lee</td><td>A Symonds</td><td>175</td>
</tr>
<tr>
<td>4</td><td>AB de Villiers</td><td>Caught NW Bracken</td><td>MJ Clarke</td><td>14</td>
</tr>
<tr>
<td>5</td><td>JH Kallis</td><td>Caught A Symonds</td><td>A Symonds</td><td>20</td>
</tr>
<tr>
<td>6</td><td>MV Boucher</td><td>NotOut  </td><td> </td><td>50</td>
</tr>
<tr>
<td>7</td><td>JM Kemp</td><td>Caught DR Martyn</td><td>NW Bracken</td><td>13</td>
</tr>
<tr>
<td>8</td><td>JJ van der Wath</td><td>Caught RT Ponting</td><td>NW Bracken</td><td>35</td>
</tr>
<tr>
<td>9</td><td>R Telemachus</td><td>Caught MEK Hussey</td><td>NW Bracken</td><td>12</td>
</tr>
<tr>
<td>10</td><td>AJ Hall</td><td>Caught MJ Clarke</td><td>B Lee</td><td>7</td>
</tr>
<tr>
<td>11</td><td>M Ntini</td><td>NotOut  </td><td> </td><td>1</td>
</tr>
<tr>
<td></td><td></td><td></td><td>Batting Total</td><td>418</td>
</tr>
<tr>
<td></td><td></td><td></td><td>Total Extras </td><td>20</td>
</tr>
<tr>
<td></td><td></td><td></td><td>Total</td><td>438</td>
</tr>
</tbody>
</table>
<h3>Extras</h3>
<table>
<thead><tr>
<th scope=""col""></th><th></th>
</tr></thead>
<tbody>
<tr>
<th scope=""row"">Byes</th><td>4</td>
</tr>
<tr>
<th scope=""row"">Leg Byes</th><td>8</td>
</tr>
<tr>
<th scope=""row"">Wides</th><td>4</td>
</tr>
<tr>
<th scope=""row"">No Balls</th><td>4</td>
</tr>
<tr>
<th scope=""row"">Penalties</th><td>0</td>
</tr>
<tr>
<th scope=""row"">Total Extras</th><td>20</td>
</tr>
</tbody>
</table>
<h3>Partnerships</h3>
<table>
<thead><tr>
<th scope=""col"">Wicket</th><th>FallOfWicket</th><th>ManOut</th>
</tr></thead>
<tbody>
<tr>
<td>1</td><td>3</td><td>2</td>
</tr>
<tr>
<td>2</td><td>190</td><td>1</td>
</tr>
<tr>
<td>3</td><td>284</td><td>4</td>
</tr>
<tr>
<td>4</td><td>299</td><td>3</td>
</tr>
<tr>
<td>5</td><td>327</td><td>5</td>
</tr>
<tr>
<td>6</td><td>355</td><td>7</td>
</tr>
<tr>
<td>7</td><td>399</td><td>8</td>
</tr>
<tr>
<td>8</td><td>423</td><td>9</td>
</tr>
<tr>
<td>9</td><td>433</td><td>10</td>
</tr>
</tbody>
</table>
<h3>Bowling</h3>
<table>
<thead><tr>
<th scope=""col"">Bowler</th><th>Wides</th><th>NB</th><th>Overs</th><th>Mdns</th><th>Runs</th><th>Wkts</th><th>Avg</th>
</tr></thead>
<tbody>
<tr>
<td>B Lee</td><td>1</td><td>3</td><td>7</td><td>0</td><td>68</td><td>1</td><td>68</td>
</tr>
<tr>
<td>NW Bracken</td><td>0</td><td>0</td><td>10</td><td>0</td><td>67</td><td>5</td><td>13.4</td>
</tr>
<tr>
<td>SR Clark</td><td>0</td><td>0</td><td>6</td><td>0</td><td>54</td><td>0</td><td>∞</td>
</tr>
<tr>
<td>ML Lewis</td><td>1</td><td>1</td><td>10</td><td>0</td><td>113</td><td>0</td><td>∞</td>
</tr>
<tr>
<td>A Symonds</td><td>0</td><td>0</td><td>9</td><td>0</td><td>75</td><td>2</td><td>37.5</td>
</tr>
<tr>
<td>MJ Clarke</td><td>0</td><td>0</td><td>7</td><td>0</td><td>49</td><td>1</td><td>49</td>
</tr>
<tr>
<td>Bowling Totals</td><td>2</td><td>4</td><td>49</td><td>0</td><td>426</td><td>9</td><td>47.33</td>
</tr>
</tbody>
</table>
<h3>Score</h3>
<p>Final Score: 438 for 9</p>

<h2>Result</h2>
<p>Match Result:  South Africa beat Australia by 1 wickets.</p>
</body>
</html>

";

        const string mdScorecard = @"# South Africa vs Australia. Venue: Joburg. Date: 12/03/2006 00:00:00. Type of Match: League
## Innings of: Australia.
### Batting
|    | Batsman      | How Out             | Bowler        | Total |
| -- | ------------ | ------------------- | ------------- | ----- |
| 1  | AC Gilchrist | Caught AJ Hall      | R Telemachus  | 55    |
| 2  | SM Katich    | Caught R Telemachus | M Ntini       | 79    |
| 3  | RT Ponting   | Caught HH Dippenaar | R Telemachus  | 164   |
| 4  | MEK Hussey   | Caught M Ntini      | AJ Hall       | 81    |
| 5  | A Symonds    | NotOut              |               | 27    |
| 6  | B Lee        | NotOut              |               | 9     |
| 7  | DR Martyn    | DidNotBat           |               | 0     |
| 8  | MJ Clarke    | DidNotBat           |               | 0     |
| 9  | NW Bracken   | DidNotBat           |               | 0     |
| 10 | SR Clark     | DidNotBat           |               | 0     |
| 11 | ML Lewis     | DidNotBat           |               | 0     |
|    |              |                     | Batting Total | 415   |
|    |              |                     | Total Extras  | 19    |
|    |              |                     | Total         | 434   |
### Extras
|              |    |
| ------------ | -- |
| __Byes__     | 0  |
| __Leg Byes__ | 4  |
| __Wides__    | 5  |
| __No Balls__ | 10 |
| __Penalties__ | 0  |
| __Total Extras__ | 19 |
### Partnerships
| Wicket | FallOfWicket | ManOut |
| ------ | ------------ | ------ |
| 1      | 97           | 1      |
| 2      | 216          | 2      |
| 3      | 374          | 4      |
| 4      | 407          | 3      |
### Bowling
| Bowler          | Wides | NB | Overs | Mdns | Runs | Wkts | Avg   |
| --------------- | ----- | -- | ----- | ---- | ---- | ---- | ----- |
| M Ntini         | 1     | 0  | 9     | 0    | 80   | 1    | 80    |
| AJ Hall         | 0     | 2  | 10    | 0    | 80   | 1    | 80    |
| JJ van der Wath | 1     | 1  | 10    | 0    | 76   | 0    | ∞     |
| R Telemachus    | 3     | 7  | 10    | 1    | 87   | 2    | 43.5  |
| GC Smith        | 0     | 0  | 4     | 0    | 29   | 0    | ∞     |
| JH Kallis       | 0     | 0  | 6     | 0    | 70   | 0    | ∞     |
| JM Kemp         | 0     | 0  | 1     | 0    | 8    | 0    | ∞     |
| Bowling Totals  | 5     | 10 | 50    | 1    | 430  | 4    | 107.5 |
### Score
Final Score: 434 for 4

## Innings of: South Africa.
### Batting
|    | Batsman         | How Out           | Bowler        | Total |
| -- | --------------- | ----------------- | ------------- | ----- |
| 1  | GC Smith        | Caught MEK Hussey | SR Clarke     | 90    |
| 2  | HH Dippenaar    | Bowled            | NW Bracken    | 1     |
| 3  | HH Gibbs        | Caught B Lee      | A Symonds     | 175   |
| 4  | AB de Villiers  | Caught NW Bracken | MJ Clarke     | 14    |
| 5  | JH Kallis       | Caught A Symonds  | A Symonds     | 20    |
| 6  | MV Boucher      | NotOut            |               | 50    |
| 7  | JM Kemp         | Caught DR Martyn  | NW Bracken    | 13    |
| 8  | JJ van der Wath | Caught RT Ponting | NW Bracken    | 35    |
| 9  | R Telemachus    | Caught MEK Hussey | NW Bracken    | 12    |
| 10 | AJ Hall         | Caught MJ Clarke  | B Lee         | 7     |
| 11 | M Ntini         | NotOut            |               | 1     |
|    |                 |                   | Batting Total | 418   |
|    |                 |                   | Total Extras  | 20    |
|    |                 |                   | Total         | 438   |
### Extras
|              |    |
| ------------ | -- |
| __Byes__     | 4  |
| __Leg Byes__ | 8  |
| __Wides__    | 4  |
| __No Balls__ | 4  |
| __Penalties__ | 0  |
| __Total Extras__ | 20 |
### Partnerships
| Wicket | FallOfWicket | ManOut |
| ------ | ------------ | ------ |
| 1      | 3            | 2      |
| 2      | 190          | 1      |
| 3      | 284          | 4      |
| 4      | 299          | 3      |
| 5      | 327          | 5      |
| 6      | 355          | 7      |
| 7      | 399          | 8      |
| 8      | 423          | 9      |
| 9      | 433          | 10     |
### Bowling
| Bowler         | Wides | NB | Overs | Mdns | Runs | Wkts | Avg   |
| -------------- | ----- | -- | ----- | ---- | ---- | ---- | ----- |
| B Lee          | 1     | 3  | 7     | 0    | 68   | 1    | 68    |
| NW Bracken     | 0     | 0  | 10    | 0    | 67   | 5    | 13.4  |
| SR Clark       | 0     | 0  | 6     | 0    | 54   | 0    | ∞     |
| ML Lewis       | 1     | 1  | 10    | 0    | 113  | 0    | ∞     |
| A Symonds      | 0     | 0  | 9     | 0    | 75   | 2    | 37.5  |
| MJ Clarke      | 0     | 0  | 7     | 0    | 49   | 1    | 49    |
| Bowling Totals | 2     | 4  | 49    | 0    | 426  | 9    | 47.33 |
### Score
Final Score: 438 for 9

## Result
Match Result: South Africa beat Australia by 1 wickets.

";

        [TestCase("HighestODIChase", DocumentType.Html)]
        [TestCase("HighestODIChase", DocumentType.Md)]
        public void UndoStuff(string index, DocumentType docType)
        {
            string matchScorecard = docType == DocumentType.Html ? HtmlScorecard : mdScorecard;
            CricketMatch friendlyString = CricketMatch.CreateFromScorecard(docType, matchScorecard);
            Assert.AreEqual(TestCaseInstances.ExampleMatches[index], friendlyString);
        }
    }
}
