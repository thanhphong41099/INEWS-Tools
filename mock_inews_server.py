import xml.etree.ElementTree as ET
from flask import Flask, request, Response

app = Flask(__name__)

# Sample story NSML to return in GetStories
STORY_NSML = """<nsml:nsml xmlns:nsml="http://avid.com/nsml" version="3.1">
	<head>
		<formname>VTV-SF</formname>
		<storyid>1b62c310:0198e185:698923ca</storyid>
	</head>
	<fields>
		<string id="title">MOCK STORY 1</string>
		<string id="video-id">TS020926BKC</string>
        <string id="page-number">26</string>
	</fields>
	<body>
		<p><cc>##CG:</cc></p>
		<p><cc>MOCK CONTENT</cc></p>
	</body>
</nsml:nsml>""".replace("<", "&lt;").replace(">", "&gt;")

def create_soap_response(inner_xml):
    return f"""<?xml version="1.0" encoding="utf-8"?>
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/" 
               xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" 
               xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <soap:Body>
    {inner_xml}
  </soap:Body>
</soap:Envelope>"""

@app.route('/inewswebservice/services/inewssystem', methods=['POST'])
def inewssystem():
    data = request.data.decode('utf-8')
    print("--- INEWSSystem Request ---")
    print(data)
    
    if "Connect" in data and "Disconnect" not in data and "IsConnected" not in data:
        resp = '<ConnectResponse xmlns="http://avid.com/inewssystem/types"></ConnectResponse>'
        return Response(create_soap_response(resp), mimetype='text/xml')
        
    if "IsConnected" in data:
        resp = '<IsConnectedResponse xmlns="http://avid.com/inewssystem/types"><IsConnected>true</IsConnected></IsConnectedResponse>'
        return Response(create_soap_response(resp), mimetype='text/xml')
        
    if "Disconnect" in data:
        resp = '<DisconnectResponse xmlns="http://avid.com/inewssystem/types"></DisconnectResponse>'
        return Response(create_soap_response(resp), mimetype='text/xml')

    if "GetFolderChildren" in data:
        # Mocking 2 child queues
        resp = """<GetFolderChildrenResponse xmlns="http://avid.com/inewssystem/types">
            <Children><Name>BAN_TIN_9H00</Name></Children>
            <Children><Name>BAN_TIN_12H00</Name></Children>
        </GetFolderChildrenResponse>"""
        return Response(create_soap_response(resp), mimetype='text/xml')

    # Default fallback
    return Response(create_soap_response('<UnknownResponse />'), mimetype='text/xml')

@app.route('/inewswebservice/services/inewsqueue', methods=['POST'])
def inewsqueue():
    data = request.data.decode('utf-8')
    print("--- INEWSQueue Request ---")
    print(data)
    
    if "SetCurrentQueue" in data:
        resp = '<SetCurrentQueueResponse xmlns="http://avid.com/inewsqueue/types"></SetCurrentQueueResponse>'
        return Response(create_soap_response(resp), mimetype='text/xml')
        
    if "GetStories" in data:
        # Mocking 2 stories
        resp = f"""<GetStoriesResponse xmlns="http://avid.com/inewsqueue/types">
            <Stories>
                <StoryAsNSML>{STORY_NSML}</StoryAsNSML>
            </Stories>
            <Stories>
                <StoryAsNSML>{STORY_NSML}</StoryAsNSML>
            </Stories>
        </GetStoriesResponse>"""
        return Response(create_soap_response(resp), mimetype='text/xml')

    # Default fallback
    return Response(create_soap_response('<UnknownResponse />'), mimetype='text/xml')

if __name__ == '__main__':
    print("Starting iNews Mock Server on port 8080...")
    app.run(host='0.0.0.0', port=8080)
