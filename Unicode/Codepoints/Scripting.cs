// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details

namespace Soe.Unicode
{
    #if EXPORT_HAMPER_CORE_UNICODE
    public
    #else
    internal
    #endif
    static class Scripting
    {
        // ReSharper disable InvalidXmlDocComment
        
        public enum LineBreak : int
        {
            /// <summary>
            /// LINE FEED (LF) \n
            /// </summary>
            LineFeed = BasicLatin.Control.LineFeed,
            /// <summary>
            /// LINE TABULATION  \v
            /// </summary>
            LineTabulation = BasicLatin.Control.LineTabulation,
            /// <summary>
            /// FORM FEED (FF)  \f
            /// </summary>
            FormFeed = BasicLatin.Control.FormFeed,
            /// <summary>
            /// CARRIAGE RETURN (CR) \r
            /// </summary>
            CarriageReturn = BasicLatin.Control.CarriageReturn,
            /// <summary>
            /// NEXT LINE (NEL) 
            /// </summary>
            NextLine = Latin1Supplement.Control.NextLine,
            /// <summary>
            /// LINE SEPARATOR 

            /// </summary>
            LineSeparator = GeneralPunctuation.LineSeparator.LineSeparator,
            /// <summary>
            /// PARAGRAPH SEPARATOR 

            /// </summary>
            ParagraphSeparator = GeneralPunctuation.ParagraphSeparator.ParagraphSeparator,
        }
        public enum WhiteSpace : int
        {
            /// <summary>
            /// CHARACTER TABULATION \t
            /// </summary>
            CharacterTabulation = BasicLatin.Control.CharacterTabulation,
            /// <summary>
            /// SPACE ( )
            /// </summary>
            Space = BasicLatin.SpaceSeparator.Space,
            /// <summary>
            /// NO-BREAK SPACE  
            /// </summary>
            NoBreakSpace = Latin1Supplement.SpaceSeparator.NoBreakSpace,
            /// <summary>
            /// OGHAM SPACE MARK  
            /// </summary>
            OghamSpaceMark = Ogham.SpaceSeparator.OghamSpaceMark,
            /// <summary>
            /// EN QUAD  
            /// </summary>
            EnQuad = GeneralPunctuation.SpaceSeparator.EnQuad,
            /// <summary>
            /// EM QUAD  
            /// </summary>
            EmQuad = GeneralPunctuation.SpaceSeparator.EmQuad,
            /// <summary>
            /// EN SPACE  
            /// </summary>
            EnSpace = GeneralPunctuation.SpaceSeparator.EnSpace,
            /// <summary>
            /// EM SPACE  
            /// </summary>
            EmSpace = GeneralPunctuation.SpaceSeparator.EmSpace,
            /// <summary>
            /// THREE-PER-EM SPACE  
            /// </summary>
            ThreePerEmSpace = GeneralPunctuation.SpaceSeparator.ThreePerEmSpace,
            /// <summary>
            /// FOUR-PER-EM SPACE  
            /// </summary>
            FourPerEmSpace = GeneralPunctuation.SpaceSeparator.FourPerEmSpace,
            /// <summary>
            /// SIX-PER-EM SPACE  
            /// </summary>
            SixPerEmSpace = GeneralPunctuation.SpaceSeparator.SixPerEmSpace,
            /// <summary>
            /// FIGURE SPACE  
            /// </summary>
            FigureSpace = GeneralPunctuation.SpaceSeparator.FigureSpace,
            /// <summary>
            /// PUNCTUATION SPACE  
            /// </summary>
            PunctuationSpace = GeneralPunctuation.SpaceSeparator.PunctuationSpace,
            /// <summary>
            /// THIN SPACE  
            /// </summary>
            ThinSpace = GeneralPunctuation.SpaceSeparator.ThinSpace,
            /// <summary>
            /// HAIR SPACE  
            /// </summary>
            HairSpace = GeneralPunctuation.SpaceSeparator.HairSpace,
            /// <summary>
            /// NARROW NO-BREAK SPACE  
            /// </summary>
            NarrowNoBreakSpace = GeneralPunctuation.SpaceSeparator.NarrowNoBreakSpace,
            /// <summary>
            /// MEDIUM MATHEMATICAL SPACE  
            /// </summary>
            MediumMathematicalSpace = GeneralPunctuation.SpaceSeparator.MediumMathematicalSpace,
            /// <summary>
            /// IDEOGRAPHIC SPACE 　
            /// </summary>
            IdeographicSpace = CJKSymbolsAndPunctuation.SpaceSeparator.IdeographicSpace,
        }
        public enum UppercaseLetter : int
        {
            /// <summary>
            /// The inclusive beginning of the LATIN CAPITAL LETTER range
            /// </summary>
            Begin = A,
            /// <summary>
            /// LATIN CAPITAL LETTER A A
            /// </summary>
            A = BasicLatin.UppercaseLetter.LatinCapitalLetterA,
            /// <summary>
            /// LATIN CAPITAL LETTER B B
            /// </summary>
            B = BasicLatin.UppercaseLetter.LatinCapitalLetterB,
            /// <summary>
            /// LATIN CAPITAL LETTER C C
            /// </summary>
            C = BasicLatin.UppercaseLetter.LatinCapitalLetterC,
            /// <summary>
            /// LATIN CAPITAL LETTER D D
            /// </summary>
            D = BasicLatin.UppercaseLetter.LatinCapitalLetterD,
            /// <summary>
            /// LATIN CAPITAL LETTER E E
            /// </summary>
            E = BasicLatin.UppercaseLetter.LatinCapitalLetterE,
            /// <summary>
            /// LATIN CAPITAL LETTER F F
            /// </summary>
            F = BasicLatin.UppercaseLetter.LatinCapitalLetterF,
            /// <summary>
            /// LATIN CAPITAL LETTER G G
            /// </summary>
            G = BasicLatin.UppercaseLetter.LatinCapitalLetterG,
            /// <summary>
            /// LATIN CAPITAL LETTER H H
            /// </summary>
            H = BasicLatin.UppercaseLetter.LatinCapitalLetterH,
            /// <summary>
            /// LATIN CAPITAL LETTER I I
            /// </summary>
            I = BasicLatin.UppercaseLetter.LatinCapitalLetterI,
            /// <summary>
            /// LATIN CAPITAL LETTER J J
            /// </summary>
            J = BasicLatin.UppercaseLetter.LatinCapitalLetterJ,
            /// <summary>
            /// LATIN CAPITAL LETTER K K
            /// </summary>
            K = BasicLatin.UppercaseLetter.LatinCapitalLetterK,
            /// <summary>
            /// LATIN CAPITAL LETTER L L
            /// </summary>
            L = BasicLatin.UppercaseLetter.LatinCapitalLetterL,
            /// <summary>
            /// LATIN CAPITAL LETTER M M
            /// </summary>
            M = BasicLatin.UppercaseLetter.LatinCapitalLetterM,
            /// <summary>
            /// LATIN CAPITAL LETTER N N
            /// </summary>
            N = BasicLatin.UppercaseLetter.LatinCapitalLetterN,
            /// <summary>
            /// LATIN CAPITAL LETTER O O
            /// </summary>
            O = BasicLatin.UppercaseLetter.LatinCapitalLetterO,
            /// <summary>
            /// LATIN CAPITAL LETTER P P
            /// </summary>
            P = BasicLatin.UppercaseLetter.LatinCapitalLetterP,
            /// <summary>
            /// LATIN CAPITAL LETTER Q Q
            /// </summary>
            Q = BasicLatin.UppercaseLetter.LatinCapitalLetterQ,
            /// <summary>
            /// LATIN CAPITAL LETTER R R
            /// </summary>
            R = BasicLatin.UppercaseLetter.LatinCapitalLetterR,
            /// <summary>
            /// LATIN CAPITAL LETTER S S
            /// </summary>
            S = BasicLatin.UppercaseLetter.LatinCapitalLetterS,
            /// <summary>
            /// LATIN CAPITAL LETTER T T
            /// </summary>
            T = BasicLatin.UppercaseLetter.LatinCapitalLetterT,
            /// <summary>
            /// LATIN CAPITAL LETTER U U
            /// </summary>
            U = BasicLatin.UppercaseLetter.LatinCapitalLetterU,
            /// <summary>
            /// LATIN CAPITAL LETTER V V
            /// </summary>
            V = BasicLatin.UppercaseLetter.LatinCapitalLetterV,
            /// <summary>
            /// LATIN CAPITAL LETTER W W
            /// </summary>
            W = BasicLatin.UppercaseLetter.LatinCapitalLetterW,
            /// <summary>
            /// LATIN CAPITAL LETTER X X
            /// </summary>
            X = BasicLatin.UppercaseLetter.LatinCapitalLetterX,
            /// <summary>
            /// LATIN CAPITAL LETTER Y Y
            /// </summary>
            Y = BasicLatin.UppercaseLetter.LatinCapitalLetterY,
            /// <summary>
            /// LATIN CAPITAL LETTER Z Z
            /// </summary>
            Z = BasicLatin.UppercaseLetter.LatinCapitalLetterZ,
            /// <summary>
            /// The inclusive end of the LATIN CAPITAL LETTER range
            /// </summary>
            End = Z
        }
        public enum LowercaseLetter : int
        {
            /// <summary>
            /// The inclusive beginning of the LATIN SMALL LETTER range
            /// </summary>
            Begin = A,
            /// <summary>
            /// LATIN SMALL LETTER A a
            /// </summary>
            A = BasicLatin.LowercaseLetter.LatinSmallLetterA,
            /// <summary>
            /// LATIN SMALL LETTER B b
            /// </summary>
            B = BasicLatin.LowercaseLetter.LatinSmallLetterB,
            /// <summary>
            /// LATIN SMALL LETTER C c
            /// </summary>
            C = BasicLatin.LowercaseLetter.LatinSmallLetterC,
            /// <summary>
            /// LATIN SMALL LETTER D d
            /// </summary>
            D = BasicLatin.LowercaseLetter.LatinSmallLetterD,
            /// <summary>
            /// LATIN SMALL LETTER E e
            /// </summary>
            E = BasicLatin.LowercaseLetter.LatinSmallLetterE,
            /// <summary>
            /// LATIN SMALL LETTER F f
            /// </summary>
            F = BasicLatin.LowercaseLetter.LatinSmallLetterF,
            /// <summary>
            /// LATIN SMALL LETTER G g
            /// </summary>
            G = BasicLatin.LowercaseLetter.LatinSmallLetterG,
            /// <summary>
            /// LATIN SMALL LETTER H h
            /// </summary>
            H = BasicLatin.LowercaseLetter.LatinSmallLetterH,
            /// <summary>
            /// LATIN SMALL LETTER I i
            /// </summary>
            I = BasicLatin.LowercaseLetter.LatinSmallLetterI,
            /// <summary>
            /// LATIN SMALL LETTER J j
            /// </summary>
            J = BasicLatin.LowercaseLetter.LatinSmallLetterJ,
            /// <summary>
            /// LATIN SMALL LETTER K k
            /// </summary>
            K = BasicLatin.LowercaseLetter.LatinSmallLetterK,
            /// <summary>
            /// LATIN SMALL LETTER L l
            /// </summary>
            L = BasicLatin.LowercaseLetter.LatinSmallLetterL,
            /// <summary>
            /// LATIN SMALL LETTER M m
            /// </summary>
            M = BasicLatin.LowercaseLetter.LatinSmallLetterM,
            /// <summary>
            /// LATIN SMALL LETTER N n
            /// </summary>
            N = BasicLatin.LowercaseLetter.LatinSmallLetterN,
            /// <summary>
            /// LATIN SMALL LETTER O o
            /// </summary>
            O = BasicLatin.LowercaseLetter.LatinSmallLetterO,
            /// <summary>
            /// LATIN SMALL LETTER P p
            /// </summary>
            P = BasicLatin.LowercaseLetter.LatinSmallLetterP,
            /// <summary>
            /// LATIN SMALL LETTER Q q
            /// </summary>
            Q = BasicLatin.LowercaseLetter.LatinSmallLetterQ,
            /// <summary>
            /// LATIN SMALL LETTER R r
            /// </summary>
            R = BasicLatin.LowercaseLetter.LatinSmallLetterR,
            /// <summary>
            /// LATIN SMALL LETTER S s
            /// </summary>
            S = BasicLatin.LowercaseLetter.LatinSmallLetterS,
            /// <summary>
            /// LATIN SMALL LETTER T t
            /// </summary>
            T = BasicLatin.LowercaseLetter.LatinSmallLetterT,
            /// <summary>
            /// LATIN SMALL LETTER U u
            /// </summary>
            U = BasicLatin.LowercaseLetter.LatinSmallLetterU,
            /// <summary>
            /// LATIN SMALL LETTER V v
            /// </summary>
            V = BasicLatin.LowercaseLetter.LatinSmallLetterV,
            /// <summary>
            /// LATIN SMALL LETTER W w
            /// </summary>
            W = BasicLatin.LowercaseLetter.LatinSmallLetterW,
            /// <summary>
            /// LATIN SMALL LETTER X x
            /// </summary>
            X = BasicLatin.LowercaseLetter.LatinSmallLetterX,
            /// <summary>
            /// LATIN SMALL LETTER Y y
            /// </summary>
            Y = BasicLatin.LowercaseLetter.LatinSmallLetterY,
            /// <summary>
            /// LATIN SMALL LETTER Z z
            /// </summary>
            Z = BasicLatin.LowercaseLetter.LatinSmallLetterZ,
            /// <summary>
            /// The inclusive end of the LATIN SMALL LETTER range
            /// </summary>
            End = Z
        }
        public enum SectionMarker : int
        {
            /// <summary>
            /// LEFT PARENTHESIS (
            /// </summary>
            LeftParenthesis = BasicLatin.OpenPunctuation.LeftParenthesis,
            /// <summary>
            /// LEFT SQUARE BRACKET [
            /// </summary>
            LeftSquareBracket = BasicLatin.OpenPunctuation.LeftSquareBracket,
            /// <summary>
            /// LEFT CURLY BRACKET {
            /// </summary>
            LeftCurlyBracket = BasicLatin.OpenPunctuation.LeftCurlyBracket,
            /// <summary>
            /// LEFT ANGLE BRACKET <
            /// </summary>
            LeftAngleBracket = BasicLatin.MathSymbol.LessThanSign,
            /// <summary>
            /// RIGHT PARENTHESIS )
            /// </summary>
            RightParenthesis = BasicLatin.ClosePunctuation.RightParenthesis,
            /// <summary>
            /// RIGHT SQUARE BRACKET ]
            /// </summary>
            RightSquareBracket = BasicLatin.ClosePunctuation.RightSquareBracket,
            /// <summary>
            /// RIGHT CURLY BRACKET }
            /// </summary>
            RightCurlyBracket = BasicLatin.ClosePunctuation.RightCurlyBracket,
            /// <summary>
            /// RIGHT ANGLE BRACKET >
            /// </summary>
            RightAngleBracket = BasicLatin.MathSymbol.GreaterThanSign
        }
        public enum Punctuation : int
        {
            /// <summary>
            /// EXCLAMATION MARK !
            /// </summary>
            ExclamationMark = BasicLatin.OtherPunctuation.ExclamationMark,
            /// <summary>
            /// QUOTATION MARK "
            /// </summary>
            QuotationMark = BasicLatin.OtherPunctuation.QuotationMark,
            /// <summary>
            /// NUMBER SIGN #
            /// </summary>
            NumberSign = BasicLatin.OtherPunctuation.NumberSign,
            /// <summary>
            /// PERCENT SIGN %
            /// </summary>
            PercentSign = BasicLatin.OtherPunctuation.PercentSign,
            /// <summary>
            /// AMPERSAND &
            /// </summary>
            Ampersand = BasicLatin.OtherPunctuation.Ampersand,
            /// <summary>
            /// APOSTROPHE '
            /// </summary>
            Apostrophe = BasicLatin.OtherPunctuation.Apostrophe,
            /// <summary>
            /// ASTERISK *
            /// </summary>
            Asterisk = BasicLatin.OtherPunctuation.Asterisk,
            /// <summary>
            /// COMMA ,
            /// </summary>
            Comma = BasicLatin.OtherPunctuation.Comma,
            /// <summary>
            /// FULL STOP .
            /// </summary>
            FullStop = BasicLatin.OtherPunctuation.FullStop,
            /// <summary>
            /// DOT .
            /// </summary>
            Dot = FullStop,
            /// <summary>
            /// SOLIDUS /
            /// </summary>
            Solidus = BasicLatin.OtherPunctuation.Solidus,
            /// <summary>
            /// COLON :
            /// </summary>
            Colon = BasicLatin.OtherPunctuation.Colon,
            /// <summary>
            /// SEMICOLON ;
            /// </summary>
            Semicolon = BasicLatin.OtherPunctuation.Semicolon,
            /// <summary>
            /// QUESTION MARK ?
            /// </summary>
            QuestionMark = BasicLatin.OtherPunctuation.QuestionMark,
            /// <summary>
            /// COMMERCIAL AT @
            /// </summary>
            CommercialAt = BasicLatin.OtherPunctuation.CommercialAt,
            /// <summary>
            /// REVERSE SOLIDUS \
            /// </summary>
            ReverseSolidus = BasicLatin.OtherPunctuation.ReverseSolidus,
            /// <summary>
            /// DOLLAR SIGN $
            /// </summary>
            DollarSign = BasicLatin.CurrencySymbol.DollarSign,
            /// <summary>
            /// PLUS SIGN +
            /// </summary>
            PlusSign = BasicLatin.MathSymbol.PlusSign,
            /// <summary>
            /// LESS-THAN SIGN <
            /// </summary>
            LessThanSign = BasicLatin.MathSymbol.LessThanSign,
            /// <summary>
            /// EQUALS SIGN =
            /// </summary>
            EqualsSign = BasicLatin.MathSymbol.EqualsSign,
            /// <summary>
            /// GREATER-THAN SIGN >
            /// </summary>
            GreaterThanSign = BasicLatin.MathSymbol.GreaterThanSign,
            /// <summary>
            /// VERTICAL LINE |
            /// </summary>
            VerticalLine = BasicLatin.MathSymbol.VerticalLine,
            /// <summary>
            /// TILDE ~
            /// </summary>
            Tilde = BasicLatin.MathSymbol.Tilde,
            /// <summary>
            /// HYPHEN-MINUS -
            /// </summary>
            HyphenMinus = BasicLatin.DashPunctuation.HyphenMinus,
            /// <summary>
            /// CIRCUMFLEX ACCENT ^
            /// </summary>
            CircumflexAccent = BasicLatin.ModifierSymbol.CircumflexAccent,
            /// <summary>
            /// LOW LINE _
            /// </summary>
            LowLine = BasicLatin.ConnectorPunctuation.LowLine
        }
        public enum Digit : int
        {
            /// <summary>
            /// The inclusive beginning of the DIGIT range
            /// </summary>
            Begin = Zero,
            /// <summary>
            /// DIGIT ZERO 0
            /// </summary>
            Zero = BasicLatin.DecimalDigitNumber.DigitZero,
            /// <summary>
            /// DIGIT ONE 1
            /// </summary>
            One = BasicLatin.DecimalDigitNumber.DigitOne,
            /// <summary>
            /// DIGIT TWO 2
            /// </summary>
            Two = BasicLatin.DecimalDigitNumber.DigitTwo,
            /// <summary>
            /// DIGIT THREE 3
            /// </summary>
            Three = BasicLatin.DecimalDigitNumber.DigitThree,
            /// <summary>
            /// DIGIT FOUR 4
            /// </summary>
            Four = BasicLatin.DecimalDigitNumber.DigitFour,
            /// <summary>
            /// DIGIT FIVE 5
            /// </summary>
            Five = BasicLatin.DecimalDigitNumber.DigitFive,
            /// <summary>
            /// DIGIT SIX 6
            /// </summary>
            Six = BasicLatin.DecimalDigitNumber.DigitSix,
            /// <summary>
            /// DIGIT SEVEN 7
            /// </summary>
            Seven = BasicLatin.DecimalDigitNumber.DigitSeven,
            /// <summary>
            /// DIGIT EIGHT 8
            /// </summary>
            Eight = BasicLatin.DecimalDigitNumber.DigitEight,
            /// <summary>
            /// DIGIT NINE 9
            /// </summary>
            Nine = BasicLatin.DecimalDigitNumber.DigitNine,
            /// <summary>
            /// The inclusive end of the DIGIT range
            /// </summary>
            End = Nine
        }
        
        // ReSharper restore InvalidXmlDocComment
    }
}