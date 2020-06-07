parser grammar MiScriptParser;

options { tokenVocab = MiScriptLexer; }

script
    : (statement eos)*
    ;

blockStatement
    : (statement (eos statement)*)?
    ;
    
statement
    : expressionStatement
    | ifStatement
    | setStatement
    | callStatement
    ;

expressionStatement
    : expression
    ;

setStatement
    : Var IdentifierStart? singleIdentifier (Is | Equal) (call | expression)
    ;

callStatement
    : call
    ;

call
    : functionName ParenOpen expression* ParenClose
    | functionName ({!lineTerminatorAhead()}? expression)*
    ;

ifStatement
    : If expression Then blockStatement elseIfStatement* elseStatement? End
    ;
    
elseIfStatement
    : Else If expression Then blockStatement
    ;

elseStatement
    : Else blockStatement
    ;
    
expression
    : IdentifierStart identifier                        # IdentifierExpression
    | string                                            # StringExpression
    | expression Equal expression                       # EqualExpression
    | expression NotEqual expression                    # NotEqualExpression
    | functionName ParenOpen expression* ParenClose     # CallExpression
    ;

identifier
    : memberIdentifier
    | singleIdentifier
    ;
    
memberIdentifier
    : Identifier (Dot (Identifier))+
    ;
    
functionName
    : Identifier
    ;
    
singleIdentifier
    : Identifier
    | Var
    | Is
    | Else
    | Then
    | End
    | If
    ;

string
    : BeginString stringPart* EndString
    ;

stringPart
    : StringText
    | Dot
    | StartExpr
    | EndExpr
    | stringEscape
    | stringIdentifier
    ;

stringEscape
    : EscapeInline
    ;

stringIdentifier
    : InlineExpr StartExpr (identifier | expression) EndExpr
    | InlineExpr identifier
    ;

eos
    : SemiColon
    | EOF
    | {lineTerminatorAhead()}?
    ;