﻿using System;

namespace loxsharp
{
    enum TokenType
    {
        // single char types
        LEFT_PAREN, RIGHT_PAREN,
        LEFT_BRACE, RIGHT_BRACE,
        LEFT_BRACKET, RIGHT_BRACKET,
        COMMA, SEMICOLON, DOT,
        STAR, PLUS, MINUS, SLASH,

        // ONE OR TWO CHARACTER TOKENS
        BANG, BANG_EQUAL,
        EQUAL, EQUAL_EQUAL,
        GREATER, GREATER_EQUAL,
        LESS, LESS_EQUAL,

        // LITERALS
        IDENTIFIER, STRING, NUMBER,

        // KEYWORDS
        AND, CLASS, ELSE, FALSE, FUN, FOR, IF, NIL, OR,
        PRINT, RETURN, SUPER, THIS, TRUE, VAR, WHILE,

        EOF
    }
}