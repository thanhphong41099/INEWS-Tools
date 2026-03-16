import xml.etree.ElementTree as ET
from flask import Flask, request, Response
import random
import string

app = Flask(__name__)

def generate_story_nsml(title="MOCK STORY", video_id=None, page_number=None):
    if not video_id:
        video_id = "TS" + "".join(random.choices(string.digits, k=6)) + "".join(random.choices(string.ascii_uppercase, k=3))
    if not page_number:
        page_number = str(random.randint(1, 100))
        
    return f"""<nsml:nsml xmlns:nsml="http://avid.com/nsml" version="3.1">
	<head>
		<formname>VTV-SF</formname>
		<storyid>1b62c310:0198e185:698923ca</storyid>
	</head>
	<fields>
		<string id="title">{title}</string>
		<string id="video-id">{video_id}</string>
        <string id="page-number">{page_number}</string>
	</fields>
	<body>
		<p><cc>##CG:</cc></p>
		<p><cc>MOCK CONTENT</cc></p>
	</body>
</nsml:nsml>"""

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
        import re
        match = re.search(r'<([^:]+:)?FolderFullName[^>]*>(.*?)</([^:]+:)?FolderFullName>', data)
        parent_folder = match.group(2) if match else "VO_BAN_TIN"
        # Mocking 2 child queues
        resp = f"""<GetFolderChildrenResponse xmlns="http://avid.com/inewssystem/types">
            <ParentFolderFullName>{parent_folder}</ParentFolderFullName>
            <Children>
                <Type>Queue</Type>
                <Name>BAN_TIN_9H00</Name>
                <FullName>{parent_folder}.BAN_TIN_9H00</FullName>
            </Children>
            <Children>
                <Type>Queue</Type>
                <Name>BAN_TIN_12H00</Name>
                <FullName>{parent_folder}.BAN_TIN_12H00</FullName>
            </Children>
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
        story1_nsml = generate_story_nsml(title="MOCK STORY 1")
        story2_nsml = generate_story_nsml(title="MOCK STORY 2")
        
        resp = f"""<GetStoriesResponse xmlns="http://avid.com/inewsqueue/types" xmlns:story="http://avid.com/inewsstory/types">
            <Stories>
                <story:FullPath>BAN_TIN_9H00</story:FullPath>
                <story:QueueLocator>1b62c310:0198e185:698923ca</story:QueueLocator>
                <story:StoryAsNSML><![CDATA[{story1_nsml}]]></story:StoryAsNSML>
            </Stories>
            <Stories>
                <story:FullPath>BAN_TIN_9H00</story:FullPath>
                <story:QueueLocator>1b62c310:0198e185:698923cb</story:QueueLocator>
                <story:StoryAsNSML><![CDATA[{story2_nsml}]]></story:StoryAsNSML>
            </Stories>
        </GetStoriesResponse>"""
        return Response(create_soap_response(resp), mimetype='text/xml')

    # Default fallback
    return Response(create_soap_response('<UnknownResponse />'), mimetype='text/xml')

if __name__ == '__main__':
    print("Starting iNews Mock Server on port 8080...")
    app.run(host='0.0.0.0', port=8080)
