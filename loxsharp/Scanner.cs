using System;
using System.Collections.Generic;

namespace loxsharp
{
    internal class Scanner
    {
        private string source;
        private List<Token> tokens = new List<Token>();
        private int line;
        private int start;
        private int current;
        private int lexLength;
        private Dictionary<string, TokenType> reservedWords;

        public Scanner(string with)
        {
            this.source = with;
            line = 1;
            start = 0;
            current = 0;
            lexLength = 0;
            reservedWords = FillKeywordDictionary();
        }

        private Dictionary<string, TokenType> FillKeywordDictionary()
        {
            return new Dictionary<string, TokenType>
            {
                {"and",TokenType.AND },
                {"class", TokenType.CLASS },
                {"else", TokenType.ELSE },
                {"false", TokenType.FALSE },
                {"fun", TokenType.FUN },
                {"for", TokenType.FOR },
                {"if", TokenType.IF },
                {"nil", TokenType.NIL },
                {"or", TokenType.OR },
                {"print", TokenType.PRINT },
                {"return", TokenType.RETURN },
                {"super", TokenType.SUPER },
                {"this", TokenType.THIS },
                {"true", TokenType.TRUE },
                {"var", TokenType.VAR },
                {"while", TokenType.WHILE },
            }; 
        }

        internal List<Token> AllTokens()
        {
            while (!IsAtEnd())
            {
                start = current;
                ScanToken();
            }
            tokens.Add(new Token(TokenType.EOF, "",null, line));
            return tokens;
        }

        private void ScanToken()
        {
            char c = ConsumeChar();
            switch (c)
            {
                case '(': AddToken(TokenType.LEFT_PAREN);
                    break;
                case ')': AddToken(TokenType.RIGHT_PAREN);
                    break;
                case '{': AddToken(TokenType.LEFT_BRACE);
                    break;
                case '}': AddToken(TokenType.RIGHT_BRACE);
                    break;
                case ',': AddToken(TokenType.COMMA);
                    break;
                case ';': AddToken(TokenType.SEMICOLON);
                    break;
                case '.': AddToken(TokenType.DOT);
                    break;
                case '*': AddToken(TokenType.STAR);
                    break;
                case '+': AddToken(TokenType.PLUS);
                    break;
                case '-': AddToken(TokenType.MINUS);
                    break;
                case '!': AddToken(MatchNext(to: '=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                    break;
                case '<': AddToken(MatchNext(to: '=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                    break;
                case '>': AddToken(MatchNext(to: '=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                    break;
                case '=': AddToken(MatchNext(to: '=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                    break;
                case '/':
                    {
                        if (MatchNext(to: '/'))
                        {
                            SkipComment();
                        }
                        else
                        {
                            AddToken(TokenType.SLASH);
                        }
                        break;
                    }
                case ' ':
                case '\r':
                case '\t':
                    ResetLexLength();
                    break;
                case '\n':
                    {
                        ResetLexLength();
                        line++;
                        break;
                    }
                case '"': AddStringToken();
                    break;
                default:
                    {
                        if (char.IsDigit(c))
                        {
                            AddNumberToken();
                        }
                        else if (c.IsAlpha())
                        {
                            AddIdentifierToken();
                        }
                        else
                        {
                            Program.Error(on: line, withMessage: $"Unexpected character: {c}");
                            ResetLexLength();
                        }
                        break;
                    }
            }
        }

        private void AddIdentifierToken()
        {
            while (Peek().IsAlpha())
            {
                ConsumeChar();
            }
            var lexeme = source.Substring(start, lexLength);
            if (reservedWords.TryGetValue(lexeme, out TokenType type))
            {
                AddToken(type, lexeme);
            }
            else
            {
                AddToken(TokenType.IDENTIFIER, lexeme);
            }
            
        }

        private void AddNumberToken()
        {
            ConsumeDigits();
            if (Peek().Equals('.') && char.IsDigit(Peek(extra:1)))
            {
                ConsumeChar();
                ConsumeDigits();
            }
            AddToken(TokenType.NUMBER, literal: double.Parse(source.Substring(start, lexLength)));
        }
        private void ConsumeDigits()
        {
            while (char.IsDigit(Peek()))
            {
                ConsumeChar();
            }
        }
        private void AddStringToken()
        {
            while (!MatchNext(to: '"') && !IsAtEnd())
            {
                if (Peek().Equals('\n'))
                {
                    line++;
                }
                ConsumeChar();
            }
            if (IsAtEnd())
            {
                Program.Error(on: line, withMessage: "Unterminated String");
            }
            var value = source.Substring(start + 1, lexLength-2);
            AddToken(TokenType.STRING,literal: value);
        }

        private void SkipComment()
        {
            while (Peek() != '\n' && !IsAtEnd())
            {
                ConsumeChar();
            }
            ResetLexLength();
        }

        private bool MatchNext(char to)
        {
            if (IsAtEnd())
            {
                return false;
            }
            if (to.Equals(Peek()))
            {
                ConsumeChar();
                return true;
            }
            return false;
        }

        private char Peek(int extra = 0)
        {
            var peekIndex = current + extra;
            return peekIndex >= source.Length ? '\0' : source[peekIndex];
        }

        private void ResetLexLength()
        {
            lexLength = 0;
        }

        private void AddToken(TokenType type, object literal = null)
        {
            var text = source.Substring(start, lexLength);
            var token = new Token(type, text,literal, line);
            tokens.Add(token);
            ResetLexLength();
        }

        private char ConsumeChar()
        {
            current++;
            lexLength++;
            return source[current - 1];
        }

        private bool IsAtEnd()
        {
            return current >= source.Length;
        }

    }
}