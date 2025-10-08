#!/bin/bash
# Move specification documents from csharp/ to root
# These are language-agnostic behavioral specifications

cd /mnt/c/Git/MarcusMedina/PackageProjects/FluentBuilders.Text

echo "Moving specification documents to root..."

mv csharp/CASING_RULES.md . 2>/dev/null && echo "✅ Moved CASING_RULES.md"
mv csharp/VALIDATION_RULES.md . 2>/dev/null && echo "✅ Moved VALIDATION_RULES.md"
mv csharp/PATTERN_RULES.md . 2>/dev/null && echo "✅ Moved PATTERN_RULES.md"
mv csharp/EXTRACTION_RULES.md . 2>/dev/null && echo "✅ Moved EXTRACTION_RULES.md"
mv csharp/COUNTING_RULES.md . 2>/dev/null && echo "✅ Moved COUNTING_RULES.md"
mv csharp/MANIPULATION_RULES.md . 2>/dev/null && echo "✅ Moved MANIPULATION_RULES.md"
mv csharp/LINEENDING_RULES.md . 2>/dev/null && echo "✅ Moved LINEENDING_RULES.md"
mv csharp/DATAFORMAT_RULES.md . 2>/dev/null && echo "✅ Moved DATAFORMAT_RULES.md"

echo ""
echo "✅ All specification documents moved to root"
echo "These specifications are now shared across all platform implementations (C#, Node.js, Python)"
