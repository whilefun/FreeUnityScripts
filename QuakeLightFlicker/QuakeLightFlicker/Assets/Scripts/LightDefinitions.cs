//
// "Quake" light styles for reference from here: https://github.com/id-Software/Quake/blob/master/QW/progs/world.qc#L303
// Other custom light styles sort of just made up for fun
//
public class LightDefinitions
{

    public const float DEFAULT_LOOP_DURATION_IN_SECONDS = 2.0f;

    public enum eLightStyles
    {

        // "Quake" light styles
        NORMAL = 0,
        FLICKER_A = 1,
        FLICKER_B = 2,
        FLOURESCENT_FLICKER = 3,
        SLOW_STRONG_PULSE = 4,
        SLOW_PULSE_NO_FADE_TO_BLACK = 5,
        GENTLE_PULSE = 6,
        CANDLE_1 = 7,
        CANDLE_2 = 8,
        CANDLE_3 = 9,
        FAST_STROBE = 10,
        SLOW_STROBE = 11,

        // Other custom styles
        CUSTOM_A = 12,
        BLACKBERRY_OS_ERROR = 13,
        FUN_ALPHABET = 14

    }
    

    //
    // By default, each light pattern is broken up by light pattern duration, where a is black, and z is bright
    // For example, "m" is just a medium power solid on light, where as "az" is a slow 1 second on 1 second off pattern,
    // and a string that is 20 chars long will be 1 char per 100ms, and so on
    //
    public static string[] LightStylePatterns =
    {

        // "Quake" light styles
        "m",                                                        // NORMAL
        "mmnmmommommnonmmonqnmmo",                                  // FLICKER_A
        "nmonqnmomnmomomno",                                        // FLICKER_B
        "mmamammmmammamamaaamammma",                                // FLOURESCENT_FLICKER
        "abcdefghijklmnopqrstuvwxyzyxwvutsrqponmlkjihgfedcba",      // SLOW_STRONG_PULSE
        "abcdefghijklmnopqrrqponmlkjihgfedcba",                     // SLOW_PULSE_NO_FADE_TO_BLACK
        "jklmnopqrstuvwxyzyxwvutsrqponmlkj",                        // GENTLE_PULSE
        "mmmmmaaaaammmmmaaaaaabcdefgabcdefg",                       // CANDLE_1
        "mmmaaaabcdefgmmmmaaaammmaamm",                             // CANDLE_2
        "mmmaaammmaaammmabcdefaaaammmmabcdefmmmaaaa",               // CANDLE_3
        "mamamamamama",                                             // FAST_STROBE
        "aaaaaaaazzzzzzzz",                                         // SLOW_STROBE
        
        // Other custom light styles
        "mynameisrichardandthisismylightpattern",                   // CUSTOM_A
        "zzzzzzzzaaaaaaaaaaaazzaaaaaazzaaaaaaaaaaaaaaaaaaaaaaaaaa", // BLACKBERRY_OS_ERROR
        "abcdefghijklmnopqrstuvwxyzyxwvutsrqponmlkjihgfedcba"       // FUN_ALPHABET 
    };

}
