﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaMono {

  public string texture = "Nova_Mono_128_SDF";

  public static float size =  128;
  public static float width = 4096;
  public static float height = 512;
   // x      y    w     h     xoffset   yoffset  advance
  public static Dictionary<char,float[]> info = new Dictionary<char,float[]>(){
    {'0', new float []{1792,0,99,137,13,116,72}},
    {'1', new float []{360,181,73,137,10,116,72}},
    {'2', new float []{492,181,101,136,14,116,72}},
    {'3', new float []{1204,181,99,135,13,114,72}},
    {'4', new float []{1278,0,104,137,16,116,72}},
    {'5', new float []{1303,181,99,135,13,114,72}},
    {'6', new float []{1891,0,99,137,13,116,72}},
    {'7', new float []{1003,181,101,135,15,114,72}},
    {'8', new float []{1990,0,99,137,13,116,72}},
    {'9', new float []{2089,0,99,137,13,116,72}},
    {' ', new float []{672,318,42,42,21,21,72}},
    {'!', new float []{433,181,59,137,6,116,72}},
    {'#', new float []{3374,181,107,107,18,101,72}},
    {'$', new float []{216,0,99,181,13,138,72}},
    {'%', new float []{1170,0,108,137,18,116,72}},
    {'&', new float []{779,181,115,135,21,114,72}},
    {'(', new float []{666,0,73,181,5,138,72}},
    {')', new float []{739,0,73,181,6,138,72}},
    {'*', new float []{3637,181,95,97,11,116,72}},
    {'+', new float []{3540,181,97,97,12,96,72}},
    {',', new float []{68,318,62,74,3,38,72}},
    {'-', new float []{478,318,97,52,12,73,72}},
    {'.', new float []{419,318,59,59,7,38,72}},
    {'/', new float []{0,0,108,181,18,138,72}},
    {':', new float []{3481,181,59,98,7,77,72}},
    {';', new float []{2093,181,62,113,3,77,72}},
    {'<', new float []{3732,181,97,96,12,95,72}},
    {'=', new float []{130,318,97,72,12,83,72}},
    {'>', new float []{3829,181,97,96,12,95,72}},
    {'?', new float []{2188,0,99,137,13,116,72}},
    {'@', new float []{3471,0,94,137,11,91,72}},
    {'A', new float []{2287,0,99,137,13,116,72}},
    {'B', new float []{1897,181,99,134,13,114,72}},
    {'C', new float []{2386,0,99,137,13,116,72}},
    {'D', new float []{1402,181,99,135,13,115,72}},
    {'E', new float []{1699,181,97,135,8,115,72}},
    {'F', new float []{690,181,89,136,8,115,72}},
    {'G', new float []{2485,0,99,137,13,116,72}},
    {'H', new float []{2584,0,99,137,13,116,72}},
    {'I', new float []{1996,181,97,134,12,114,72}},
    {'J', new float []{1501,181,99,135,13,114,72}},
    {'K', new float []{2683,0,99,137,13,116,72}},
    {'L', new float []{593,181,97,136,12,116,72}},
    {'M', new float []{1590,0,101,137,14,116,72}},
    {'N', new float []{2782,0,99,137,13,116,72}},
    {'O', new float []{2881,0,99,137,13,116,72}},
    {'P', new float []{1600,181,99,135,13,114,72}},
    {'Q', new float []{958,0,103,146,15,116,72}},
    {'R', new float []{1104,181,100,135,14,114,72}},
    {'S', new float []{2980,0,99,137,13,116,72}},
    {'T', new float []{894,181,109,135,18,114,72}},
    {'U', new float []{3079,0,99,137,13,116,72}},
    {'V', new float []{1061,0,109,137,18,116,72}},
    {'W', new float []{1691,0,101,137,14,116,72}},
    {'X', new float []{1382,0,104,137,16,116,72}},
    {'Y', new float []{3178,0,99,137,13,116,72}},
    {'Z', new float []{1796,181,101,134,15,114,72}},
    {'[', new float []{590,0,76,181,6,138,72}},
    {']', new float []{513,0,77,181,11,138,72}},
    {'^', new float []{227,318,93,69,10,116,72}},
    {'_', new float []{575,318,97,52,12,31,72}},
    {'`', new float []{0,318,68,74,2,116,72}},
    {'a', new float []{2561,181,90,112,9,91,72}},
    {'b', new float []{3658,0,90,137,9,116,72}},
    {'c', new float []{2651,181,90,112,9,91,72}},
    {'d', new float []{3748,0,90,137,9,116,72}},
    {'e', new float []{2741,181,90,112,9,91,72}},
    {'f', new float []{3277,0,97,137,16,116,72}},
    {'g', new float []{3838,0,90,137,9,91,72}},
    {'h', new float []{3928,0,90,137,9,116,72}},
    {'i', new float []{3565,0,93,137,12,116,72}},
    {'j', new float []{865,0,93,162,9,116,72}},
    {'k', new float []{0,181,90,137,9,116,72}},
    {'l', new float []{90,181,90,137,9,116,72}},
    {'m', new float []{2260,181,101,112,14,91,72}},
    {'n', new float []{2831,181,90,112,9,91,72}},
    {'o', new float []{2921,181,90,112,9,91,72}},
    {'p', new float []{180,181,90,137,9,91,72}},
    {'q', new float []{1486,0,104,137,9,91,72}},
    {'r', new float []{3011,181,90,112,9,91,72}},
    {'s', new float []{3191,181,89,112,8,91,72}},
    {'t', new float []{3374,0,97,137,16,116,72}},
    {'u', new float []{3101,181,90,112,9,91,72}},
    {'v', new float []{2155,181,105,112,16,91,72}},
    {'w', new float []{2361,181,101,112,14,91,72}},
    {'x', new float []{2462,181,99,112,14,91,72}},
    {'y', new float []{270,181,90,137,9,91,72}},
    {'z', new float []{3280,181,94,110,11,90,72}},
    {'{', new float []{315,0,99,181,17,138,72}},
    {'|', new float []{812,0,53,181,10,138,72}},
    {'}', new float []{414,0,99,181,10,138,72}},
    {'~', new float []{320,318,99,64,13,79,72}},
    {'"', new float []{3926,181,72,77,0,116,72}},
    {'\\', new float []{108,0,108,181,18,138,72}},
    {'\'', new float []{3998,181,53,77,10,116,72}}
  };
}





public class Inconsolata {

  public string texture = "Inconsolata_128_SDF";

  public static float size =  128;
  public static float width = 4096;
  public static float height = 512;
   // x      y    w     h     xoffset   yoffset  advance
  public static Dictionary<char,float[]> info = new Dictionary<char,float[]>(){
    {'0' , new float[]{3233,0,118,146,27,113,64}},
    {'1' , new float[]{3187,172,94,142,22,111,64}},
    {'2' , new float[]{586,172,114,144,26,113,64}},
    {'3' , new float[]{3351,0,114,146,26,113,64}},
    {'4' , new float[]{1924,172,118,142,27,111,64}},
    {'5' , new float[]{232,172,115,145,24,111,64}},
    {'6' , new float[]{3465,0,114,146,25,113,64}},
    {'7' , new float[]{2968,172,111,142,23,111,64}},
    {'8' , new float[]{2875,0,116,147,26,113,64}},
    {'9' , new float[]{3579,0,114,146,26,113,64}},
    {' ' , new float[]{2466,317,62,62,31,31,64}},
    {'!' , new float[]{1513,0,83,151,11,118,64}},
    {'#' , new float[]{1322,172,122,142,29,112,64}},
    {'$' , new float[]{1164,0,116,152,25,115,64}},
    {'%' , new float[]{3693,0,123,145,29,112,64}},
    {'&' , new float[]{2991,0,122,146,27,112,64}},
    {'(' , new float[]{108,0,103,172,18,117,64}},
    {')' , new float[]{211,0,103,172,18,117,64}},
    {'*' , new float[]{1246,317,119,117,27,101,64}},
    {'+' , new float[]{1365,317,117,116,26,99,64}},
    {',' , new float[]{1588,317,85,100,12,47,64}},
    {'-' , new float[]{2356,317,110,74,23,78,64}},
    {'.' , new float[]{2157,317,80,80,9,47,64}},
    {'/' , new float[]{936,0,114,154,25,117,64}},
    {':' , new float[]{1164,317,82,120,11,87,64}},
    {';' , new float[]{3395,172,85,140,13,87,64}},
    {'<' , new float[]{3480,172,119,132,27,106,64}},
    {'=' , new float[]{1673,317,117,97,26,90,64}},
    {'>' , new float[]{3599,172,118,132,26,106,64}},
    {'?' , new float[]{1716,0,113,150,24,117,64}},
    {'@' , new float[]{3113,0,120,146,28,113,64}},
    {'A' , new float[]{1075,172,124,142,30,111,64}},
    {'B' , new float[]{2042,172,118,142,26,111,64}},
    {'C' , new float[]{2398,0,121,147,27,113,64}},
    {'D' , new float[]{2160,172,118,142,26,111,64}},
    {'E' , new float[]{2629,172,114,142,24,111,64}},
    {'F' , new float[]{2856,172,112,142,22,111,64}},
    {'G' , new float[]{2519,0,120,147,29,113,64}},
    {'H' , new float[]{2396,172,117,142,26,111,64}},
    {'I' , new float[]{3079,172,108,142,22,111,64}},
    {'J' , new float[]{3816,0,119,145,28,111,64}},
    {'K' , new float[]{347,172,122,144,26,112,64}},
    {'L' , new float[]{2743,172,113,142,23,111,64}},
    {'M' , new float[]{1565,172,120,142,27,111,64}},
    {'N' , new float[]{1685,172,120,142,27,111,64}},
    {'O' , new float[]{2275,0,123,147,29,113,64}},
    {'P' , new float[]{2513,172,116,142,25,111,64}},
    {'Q' , new float[]{610,0,122,163,29,113,64}},
    {'R' , new float[]{1805,172,119,142,25,111,64}},
    {'S' , new float[]{2639,0,118,147,28,113,64}},
    {'T' , new float[]{1444,172,121,142,29,111,64}},
    {'U' , new float[]{469,172,117,144,26,111,64}},
    {'V' , new float[]{700,172,125,142,30,111,64}},
    {'W' , new float[]{825,172,125,142,30,111,64}},
    {'X' , new float[]{1199,172,123,142,28,111,64}},
    {'Y' , new float[]{950,172,125,142,30,111,64}},
    {'Z' , new float[]{2278,172,118,142,26,111,64}},
    {'[' , new float[]{732,0,102,159,16,117,64}},
    {']' , new float[]{834,0,102,159,16,117,64}},
    {'^' , new float[]{1482,317,106,100,21,111,64}},
    {'_' , new float[]{2237,317,119,74,27,30,64}},
    {'`' , new float[]{1961,317,82,87,12,122,64}},
    {'a' , new float[]{0,317,114,126,26,93,64}},
    {'b' , new float[]{1397,0,116,151,25,118,64}},
    {'c' , new float[]{3836,172,117,126,26,92,64}},
    {'d' , new float[]{1280,0,117,151,27,118,64}},
    {'e' , new float[]{3953,172,116,126,26,92,64}},
    {'f' , new float[]{1596,0,120,150,26,119,64}},
    {'g' , new float[]{2757,0,118,147,25,93,64}},
    {'h' , new float[]{1948,0,113,149,23,118,64}},
    {'i' , new float[]{2171,0,104,149,20,118,64}},
    {'j' , new float[]{0,0,108,172,28,118,64}},
    {'k' , new float[]{1829,0,119,149,23,118,64}},
    {'l' , new float[]{2061,0,110,149,23,118,64}},
    {'m' , new float[]{228,317,120,125,27,94,64}},
    {'n' , new float[]{462,317,113,123,23,92,64}},
    {'o' , new float[]{3717,172,119,126,27,92,64}},
    {'p' , new float[]{0,172,116,145,24,92,64}},
    {'q' , new float[]{116,172,116,145,27,92,64}},
    {'r' , new float[]{575,317,111,123,20,92,64}},
    {'s' , new float[]{114,317,114,126,25,92,64}},
    {'t' , new float[]{3281,172,114,141,24,108,64}},
    {'u' , new float[]{348,317,114,125,25,91,64}},
    {'v' , new float[]{930,317,118,122,28,91,64}},
    {'w' , new float[]{686,317,124,122,30,91,64}},
    {'x' , new float[]{810,317,120,122,28,91,64}},
    {'y' , new float[]{3935,0,119,145,30,91,64}},
    {'z' , new float[]{1048,317,116,122,26,91,64}},
    {'{' , new float[]{390,0,110,164,27,113,64}},
    {'|' , new float[]{314,0,76,165,6,115,64}},
    {'}' , new float[]{500,0,110,164,24,113,64}},
    {'~' , new float[]{2043,317,114,82,24,86,64}},
    {'\'', new float[]{1885,317,76,95,6,116,64}},
    {'"', new float[]{1790,317,95,95,15,116,64}},
    {'\\', new float[]{1050,0,114,154,25,117,64}}
  };
}





public class UbuntuMono {

  public string texture = "UbuntuMono_128_SDF";

  public static float size =  128;
  public static float width = 4096;
  public static float height = 512;
   // x      y    w     h     xoffset   yoffset  advance
  public static Dictionary<char,float[]> info = new Dictionary<char,float[]>(){

// Ubuntu Mono
    {'0', new float[]{2608,0,115,145,25,112,64}},
    {'1', new float[]{2859,174,109,142,22,111,64}},
    {'2', new float[]{534,174,112,143,24,112,64}},
    {'3', new float[]{3178,0,112,145,24,112,64}},
    {'4', new float[]{1494,174,118,142,27,111,64}},
    {'5', new float[]{3960,0,111,144,23,111,64}},
    {'6', new float[]{3508,0,113,144,24,111,64}},
    {'7', new float[]{2190,174,113,142,23,111,64}},
    {'8', new float[]{2838,0,114,145,25,112,64}},
    {'9', new float[]{3621,0,113,144,25,112,64}},
    {' ', new float[]{2235,318,62,62,31,31,64}},
    {'!', new float[]{109,174,80,144,8,111,64}},
    {'#', new float[]{1016,174,120,142,28,111,64}},
    {'$', new float[]{1109,0,113,164,24,120,64}},
    {'%', new float[]{2132,0,122,145,29,112,64}},
    {'&', new float[]{2254,0,119,145,27,112,64}},
    {'(', new float[]{0,0,98,174,17,121,64}},
    {')', new float[]{98,0,98,174,17,121,64}},
    {'*', new float[]{1136,318,112,110,24,111,64}},
    {'+', new float[]{791,318,115,118,25,93,64}},
    {',', new float[]{1365,318,89,99,12,49,64}},
    {'-', new float[]{2144,318,91,72,13,69,64}},
    {'.', new float[]{1821,318,81,82,8,49,64}},
    {'/', new float[]{196,0,109,173,22,121,64}},
    {':', new float[]{3835,174,81,124,8,91,64}},
    {';', new float[]{3179,174,89,140,15,91,64}},
    {'<', new float[]{906,318,115,113,25,89,64}},
    {'=', new float[]{1528,318,115,96,25,82,64}},
    {'>', new float[]{1021,318,115,113,25,89,64}},
    {'?', new float[]{3290,0,103,145,19,112,64}},
    {'@', new float[]{1222,0,118,161,26,112,64}},
    {'A', new float[]{646,174,124,142,30,111,64}},
    {'B', new float[]{421,174,113,143,24,111,64}},
    {'C', new float[]{2492,0,116,145,25,112,64}},
    {'D', new float[]{306,174,115,143,24,111,64}},
    {'E', new float[]{2639,174,110,142,19,111,64}},
    {'F', new float[]{2968,174,107,142,19,111,64}},
    {'G', new float[]{2723,0,115,145,25,112,64}},
    {'H', new float[]{1846,174,115,142,25,111,64}},
    {'I', new float[]{3075,174,104,142,20,111,64}},
    {'J', new float[]{0,174,109,144,24,111,64}},
    {'K', new float[]{1612,174,117,142,23,111,64}},
    {'L', new float[]{2749,174,110,142,19,111,64}},
    {'M', new float[]{1256,174,119,142,27,111,64}},
    {'N', new float[]{2303,174,113,142,24,111,64}},
    {'O', new float[]{2373,0,119,145,27,112,64}},
    {'P', new float[]{2528,174,111,142,22,111,64}},
    {'Q', new float[]{990,0,119,165,27,112,64}},
    {'R', new float[]{1961,174,115,142,24,111,64}},
    {'S', new float[]{2952,0,113,145,24,112,64}},
    {'T', new float[]{1729,174,117,142,26,111,64}},
    {'U', new float[]{3393,0,115,144,25,111,64}},
    {'V', new float[]{770,174,123,142,29,111,64}},
    {'W', new float[]{1375,174,119,142,27,111,64}},
    {'X', new float[]{1136,174,120,142,28,111,64}},
    {'Y', new float[]{893,174,123,142,29,111,64}},
    {'Z', new float[]{2076,174,114,142,24,111,64}},
    {'[', new float[]{628,0,93,173,13,121,64}},
    {'^', new float[]{1248,318,117,106,26,111,64}},
    {'_', new float[]{2019,318,125,72,30,19,64}},
    {'`', new float[]{1737,318,84,85,10,120,64}},
    {'a', new float[]{3615,174,110,125,24,92,64}},
    {'b', new float[]{1340,0,113,152,22,120,64}},
    {'c', new float[]{3501,174,114,125,25,92,64}},
    {'d', new float[]{1453,0,113,152,26,120,64}},
    {'e', new float[]{3385,174,116,125,26,92,64}},
    {'f', new float[]{1679,0,116,151,23,120,64}},
    {'g', new float[]{3065,0,113,145,26,92,64}},
    {'h', new float[]{1910,0,109,151,22,120,64}},
    {'i', new float[]{2019,0,113,150,24,117,64}},
    {'j', new float[]{887,0,103,170,22,117,64}},
    {'k', new float[]{1795,0,115,151,22,120,64}},
    {'l', new float[]{1566,0,113,152,24,119,64}},
    {'m', new float[]{3916,174,117,123,26,92,64}},
    {'n', new float[]{0,318,109,123,22,92,64}},
    {'o', new float[]{3268,174,117,125,26,92,64}},
    {'p', new float[]{3734,0,113,144,22,92,64}},
    {'q', new float[]{3847,0,113,144,26,92,64}},
    {'r', new float[]{218,318,105,123,17,92,64}},
    {'s', new float[]{3725,174,110,125,23,92,64}},
    {'t', new float[]{2416,174,112,142,23,109,64}},
    {'u', new float[]{109,318,109,123,22,91,64}},
    {'v', new float[]{445,318,119,122,27,91,64}},
    {'w', new float[]{323,318,122,122,29,91,64}},
    {'x', new float[]{564,318,119,122,27,91,64}},
    {'y', new float[]{189,174,117,143,27,91,64}},
    {'z', new float[]{683,318,108,122,22,91,64}},
    {'{', new float[]{414,0,107,173,21,121,64}},
    {'|', new float[]{814,0,73,173,4,121,64}},
    {'}', new float[]{521,0,107,173,22,121,64}},
    {'~', new float[]{1902,318,117,81,26,74,64}},   
    {'\'', new float[]{1454,318,74,97,5,118,64}},
    {'"', new float[]{1643,318,94,94,15,118,64}},
    {'\\', new float[]{305,0,109,173,22,121,64}},
    {']', new float[]{721,0,93,173,16,121,64}}
  };
}






public class ShareTech {

  public string texture = "ShareTech_128_SDF";

  public static float size =  128;
  public static float width = 4096;
  public static float height = 512;
   // x      y    w     h     xoffset   yoffset  advance
  public static Dictionary<char,float[]> info = new Dictionary<char,float[]>(){
    {'0',new float[]{547,183,108,152,19,121,69}},
    {'1',new float[]{977,183,106,152,18,121,69}},
    {'2',new float[]{1295,183,105,152,18,121,69}},
    {'3',new float[]{1920,183,101,152,16,121,69}},
    {'4',new float[]{3571,0,110,152,20,121,69}},
    {'5',new float[]{2021,183,101,152,16,121,69}},
    {'6',new float[]{763,183,107,152,19,121,69}},
    {'7',new float[]{1610,183,104,152,18,121,69}},
    {'8',new float[]{3681,0,110,152,20,121,69}},
    {'9',new float[]{870,183,107,152,19,121,69}},
    {' ',new float[]{1913,335,62,62,31,31,69}},
    {'!',new float[]{1177,0,76,153,3,121,69}},
    {'#',new float[]{3127,0,111,152,21,121,69}},
    {'$',new float[]{855,0,105,177,18,131,69}},
    {'%',new float[]{1253,0,129,152,30,121,69}},
    {'&',new float[]{2101,0,116,152,21,121,69}},
    {'(',new float[]{368,0,88,183,9,136,69}},
    {')',new float[]{456,0,88,183,9,136,69}},
    {'*',new float[]{822,335,114,111,22,124,69}},
    {'+',new float[]{710,335,112,112,21,96,69}},
    {',',new float[]{1331,335,81,93,6,44,69}},
    {'-',new float[]{1809,335,104,72,17,76,69}},
    {'.',new float[]{1607,335,75,75,3,44,69}},
    {'/',new float[]{544,0,119,180,25,135,69}},
    {':',new float[]{413,335,75,126,3,95,69}},
    {';',new float[]{2866,183,81,144,6,95,69}},
    {'<',new float[]{488,335,111,121,21,103,69}},
    {'=',new float[]{1050,335,111,98,21,89,69}},
    {'>',new float[]{599,335,111,121,21,103,69}},
    {'?',new float[]{2122,183,99,152,15,121,69}},
    {'@',new float[]{1382,0,124,152,27,121,69}},
    {'A',new float[]{1506,0,120,152,25,121,69}},
    {'B',new float[]{3791,0,110,152,20,121,69}},
    {'C',new float[]{1714,183,104,152,17,121,69}},
    {'D',new float[]{3238,0,111,152,21,121,69}},
    {'E',new float[]{1400,183,105,152,18,121,69}},
    {'F',new float[]{1505,183,105,152,18,121,69}},
    {'G',new float[]{655,183,108,152,19,121,69}},
    {'H',new float[]{3349,0,111,152,21,121,69}},
    {'I',new float[]{2320,183,97,152,14,121,69}},
    {'J',new float[]{2221,183,99,152,15,121,69}},
    {'K',new float[]{2791,0,112,152,21,121,69}},
    {'L',new float[]{1818,183,102,152,16,121,69}},
    {'M',new float[]{1865,0,118,152,24,121,69}},
    {'N',new float[]{2903,0,112,152,21,121,69}},
    {'O',new float[]{2563,0,114,152,22,121,69}},
    {'P',new float[]{220,183,109,152,20,121,69}},
    {'Q',new float[]{1057,0,120,160,22,121,69}},
    {'R',new float[]{2333,0,115,152,22,121,69}},
    {'S',new float[]{1083,183,106,152,18,121,69}},
    {'T',new float[]{2448,0,115,152,23,121,69}},
    {'U',new float[]{3015,0,112,152,21,121,69}},
    {'V',new float[]{2217,0,116,152,24,121,69}},
    {'W',new float[]{1983,0,118,152,25,121,69}},
    {'X',new float[]{1746,0,119,152,25,121,69}},
    {'Y',new float[]{1626,0,120,152,25,121,69}},
    {'Z',new float[]{329,183,109,152,20,121,69}},
    {'[',new float[]{188,0,90,183,10,136,69}},
    {']',new float[]{278,0,90,183,10,136,69}},
    {'^',new float[]{936,335,114,106,22,129,69}},
    {'_',new float[]{1682,335,127,72,29,19,69}},
    {'`',new float[]{1412,335,89,80,10,123,69}},
    {'a',new float[]{3295,183,115,126,23,95,69}},
    {'b',new float[]{3901,0,110,152,20,121,69}},
    {'c',new float[]{312,335,101,126,16,95,69}},
    {'d',new float[]{0,183,110,152,20,121,69}},
    {'e',new float[]{3861,183,108,126,19,95,69}},
    {'f',new float[]{1189,183,106,152,18,121,69}},
    {'g',new float[]{2533,183,113,148,22,95,69}},
    {'h',new float[]{110,183,110,152,20,121,69}},
    {'i',new float[]{3460,0,111,152,21,121,69}},
    {'j',new float[]{960,0,97,174,14,121,69}},
    {'k',new float[]{438,183,109,152,19,121,69}},
    {'l',new float[]{2677,0,114,152,22,121,69}},
    {'m',new float[]{3174,183,121,126,26,95,69}},
    {'n',new float[]{3751,183,110,126,20,95,69}},
    {'o',new float[]{3640,183,111,126,21,95,69}},
    {'p',new float[]{2646,183,110,148,20,95,69}},
    {'q',new float[]{2756,183,110,148,20,95,69}},
    {'r',new float[]{0,335,105,126,18,95,69}},
    {'s',new float[]{210,335,102,126,16,95,69}},
    {'t',new float[]{2947,183,104,142,17,111,69}},
    {'u',new float[]{3969,183,108,126,19,95,69}},
    {'v',new float[]{3410,183,115,126,23,95,69}},
    {'w',new float[]{3051,183,123,126,27,95,69}},
    {'x',new float[]{3525,183,115,126,23,95,69}},
    {'y',new float[]{2417,183,116,148,23,95,69}},
    {'z',new float[]{105,335,105,126,18,95,69}},
    {'{',new float[]{0,0,94,183,12,136,69}},
    {'|',new float[]{782,0,73,180,2,135,69}},
    {'}',new float[]{94,0,94,183,12,136,69}},
    {'~',new float[]{1501,335,106,78,18,80,69}},
    {'\'',new float[]{1256,335,75,94,3,121,69}},
    {'"',new float[]{1161,335,95,94,13,121,69}},
    {'\\',new float[]{663,0,119,180,25,135,69}}
  };
}