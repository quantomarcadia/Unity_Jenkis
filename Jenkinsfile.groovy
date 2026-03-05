def PROJECT_NAME = "Unity_Jenkins"
def CUSTOM_WORKSPACE = "C:\\Jenkins\\Unity_Projects\\${PROJECT_NAME}"
def UNITY_VERSION = "6000.3.6f1"
def UNITY_INSTALLATION = "C:\\Program Files\\Unity\\Hub\\Editor\\${UNITY_VERSION}\\Editor"

pipeline{
    environment{
        PROJECT_PATH = "${CUSTOM_WORKSPACE}\\${PROJECT_NAME}"
    }

    agent{
        label{
            label ""
            customWorkspace "${CUSTOM_WORKSPACE}"
        }
    }

    stages{

        stage('Build Windows'){
            when{expression{BUILD_WINDOWS == 'true'}}
            steps{
                script{
                    withEnv(["UNITY_PATH=${UNITY_INSTALLATION}"]){
                        bat '''
                        "%UNITY_PATH%\\Unity.exe" -quit -batchmode -nographics -buildTarget StandaloneWindows64 -projectPath "%PROJECT_PATH%" -executeMethod BuildScript.BuildWindows -logFile -
                        '''
                    }
                }
            }
        }

        stage('Deploy Windows'){
            when{expression{DEPLOY_WINDOWS == 'true'}}
            steps{
                echo 'Deploy Windows'
            }
        }

        stage('Build Android APK'){
            when{expression{BUILD_ANDROID_APK == 'true'}}
            steps{
                script{
                    withEnv(["UNITY_PATH=${UNITY_INSTALLATION}"]){
                        bat '''
                        "%UNITY_PATH%\\Unity.exe" -quit -batchmode -nographics -buildTarget Android -projectPath "%PROJECT_PATH%" -executeMethod BuildScript.BuildAndroid -buildType APK -logFile -
                        '''
                    }
                }
            }
        }

        stage('Deploy Android APK'){
            when{expression{DEPLOY_ANDROID_APK == 'true'}}
            steps{
                echo 'Deploy Android APK'
            }
        }

        stage('Build Android AAB'){
            when{expression{BUILD_ANDROID_AAB == 'true'}}
            steps{
                script{
                    withEnv(["UNITY_PATH=${UNITY_INSTALLATION}"]){
                        bat '''
                        "%UNITY_PATH%\\Unity.exe" -quit -batchmode -nographics -buildTarget Android -projectPath "%PROJECT_PATH%" -executeMethod BuildScript.BuildAndroid -buildType AAB -logFile -
                        '''
                    }
                }
            }
        }

        stage('Deploy Android AAB'){
            when{expression{DEPLOY_ANDROID_AAB == 'true'}}
            steps{
                echo 'Deploy Android AAB'
            }
        }

        stage('Build Linux Server'){
            when{expression{BUILD_LINUX_SERVER == 'true'}}
            steps{
                script{
                    withEnv(["UNITY_PATH=${UNITY_INSTALLATION}"]){
                        bat '''
                        "%UNITY_PATH%\\Unity.exe" -quit -batchmode -nographics -buildTarget StandaloneLinux64 -projectPath "%PROJECT_PATH%" -executeMethod BuildScript.BuildLinuxServer -logFile -
                        '''
                    }
                }
            }
        }

        stage('Deploy Linux Server'){
            when{expression{DEPLOY_LINUX_SERVER == 'true'}}
            steps{
                echo 'Deploy Linux Server'
            }
        }

    }
}


