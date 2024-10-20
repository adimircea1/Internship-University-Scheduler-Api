@echo off

echo Building authorization-authentication-image...
if exist ../AuthorizationApi/Internship.AuthorizationAuthentication.Api.Presentation/Dockerfile (
    docker build -f ../AuthorizationApi/Internship.AuthorizationAuthentication.Api.Presentation/Dockerfile --progress=plain --no-cache -t authorization-authentication-image:latest .
    if %ERRORLEVEL% neq 0 (
        echo Failed to build authorization-authentication-image!
    ) else (
        echo Successfully built authorization-authentication-image.
    )
) else (
    echo Dockerfile for authorization-authentication-image not found!
)

echo Building university-scheduler-image...
if exist ../UniversitySchedulerApi/Internship.UniversityScheduler.Api.Presentation/Dockerfile (
    docker build -f ../UniversitySchedulerApi/Internship.UniversityScheduler.Api.Presentation/Dockerfile --progress=plain --no-cache -t university-scheduler-image:latest .
    if %ERRORLEVEL% neq 0 (
        echo Failed to build university-scheduler-image!
    ) else (
        echo Successfully built university-scheduler-image.
    )
) else (
    echo Dockerfile for university-scheduler-image not found!
)

echo Building email-verification-image...
if exist ../EmailVerificationApi/EmailVerificationApi.Presentation/Dockerfile (
    docker build -f ../EmailVerificationApi/EmailVerificationApi.Presentation/Dockerfile --progress=plain --no-cache -t email-verification-image:latest .
    if %ERRORLEVEL% neq 0 (
        echo Failed to build email-verification-image!
    ) else (
        echo Successfully built email-verification-image.
    )
) else (
    echo Dockerfile for email-verification-image not found!
)

echo Building student-examination-image...
if exist ../StudentExaminationApi/StudentExamination.Api.Presentation/Dockerfile (
    docker build -f ../StudentExaminationApi/StudentExamination.Api.Presentation/Dockerfile --progress=plain --no-cache -t student-examination-image:latest .
    if %ERRORLEVEL% neq 0 (
        echo Failed to build student-examination-image!
    ) else (
        echo Successfully built student-examination-image.
    )
) else (
    echo Dockerfile for student-examination-image not found!
)

echo All builds complete!
pause
