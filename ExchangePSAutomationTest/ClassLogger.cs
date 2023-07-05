/*
 * By David Barrett, Microsoft Ltd. 2014-2022. Use at your own risk.  No warranties are given.
 * 
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 * */

using System;
using System.IO;

namespace ExchangePSAutomationTest
{
    class ClassLogger
    {
        private StreamWriter _logStream = null;
        private string _logPath = "";
        private bool _logDateAndTime = true;

        public ClassLogger(string LogFile)
        {
            try
            {
                _logStream = File.AppendText(LogFile);
                _logPath = LogFile;
            }
            catch { }
        }

        ~ClassLogger()
        {
            try
            {
                _logStream.Flush();
                _logStream.Close();
            }
            catch { }
        }

        public bool LogDateAndTime
        {
            get { return _logDateAndTime; }
            set { _logDateAndTime = value; }
        }

        public void ClearFile()
        {
        }

        public void Log(string Details, string Title = "")
        {
            try
            {
                DateTime oLogTime = DateTime.Now;
                if (!String.IsNullOrEmpty(Title))
                {
                    _logStream.WriteLine("");
                    if (_logDateAndTime)
                        _logStream.WriteLine(String.Format("{0:dd/MM/yy HH:mm:ss} ==> {1}", oLogTime, Title));
                    if (!String.IsNullOrEmpty(Details))
                        _logStream.WriteLine(Details);
                }
                else
                {
                    if (_logDateAndTime)
                        _logStream.WriteLine(String.Format("{0:dd/MM/yy HH:mm:ss}: {1}", oLogTime, Details));
                }
                _logStream.Flush();
            }
            catch {}
        }
    }
}
