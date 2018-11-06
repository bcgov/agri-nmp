FROM registry.access.redhat.com/rhel7/rhel-atomic


# https://blog.csanchez.org/2017/05/31/running-a-jvm-in-a-container-without-getting-killed/

RUN set -x && microdnf -h && \
    curl -so /etc/yum.repos.d/jenkins.repo http://pkg.jenkins-ci.org/redhat-stable/jenkins.repo && \
    rpm --import https://jenkins-ci.org/redhat/jenkins-ci.org.key && \
    microdnf --enablerepo=rhel-7-server-rpms --enablerepo=rhel-server-rhscl-7-rpms --enablerepo=jenkins install which gpg java-1.8.0-openjdk-devel shadow-utils jenkins zip unzip rsync elfutils rh-git29 --nodocs && \
    microdnf clean all && \
    echo "unset BASH_ENV PROMPT_COMMAND ENV" > /usr/local/bin/scl_enable && \
    echo "source scl_source enable rh-git29" >> /usr/local/bin/scl_enable && \
    chgrp -R 0 /usr/local/bin && \
    chmod -R g+rx /usr/local/bin && \
    rpm -qa

# When bash is started non-interactively, to run a shell script, for example it
# looks for this variable and source the content of this file. This will enable
# the SCL for all scripts without need to do 'scl enable'.
ENV BASH_ENV=/usr/local/bin/scl_enable \
    ENV=/usr/local/bin/scl_enable \
    PROMPT_COMMAND=". /usr/local/bin/scl_enable"

ARG JAVA_TOOL_OPTIONS="-XX:+UnlockExperimentalVMOptions -XX:+UseCGroupMemoryLimitForHeap -XX:MaxRAMFraction=2"
ARG OC_VERSION=v3.9.0
ARG OC_QUALIFIER=191fece-linux-64bit
#OC command line
RUN set -x && \
    mkdir /tmp/oc && \
    curl -fsSL https://github.com/openshift/origin/releases/download/${OC_VERSION}/openshift-origin-client-tools-${OC_VERSION}-${OC_QUALIFIER}.tar.gz -o /tmp/oc/openshift-origin-client-tools.tar.gz && \
    tar -xzf /tmp/oc/openshift-origin-client-tools.tar.gz -C /tmp/oc  --strip-components=1 && \
    cp /tmp/oc/oc /usr/local/bin/ && \
    chmod +x /usr/local/bin/oc && \
    rm -rf /tmp/oc

RUN set -x && \
    curl -sLo /usr/local/bin/dumb-init https://github.com/Yelp/dumb-init/releases/download/v1.2.2/dumb-init_1.2.2_amd64 && \
    chmod +x /usr/local/bin/dumb-init

ARG TINI_VERSION=v0.18.0
ARG TINI_QUALIFIER=amd64
RUN curl -fsSL https://github.com/krallin/tini/releases/download/${TINI_VERSION}/tini-static-${TINI_QUALIFIER} -o /sbin/tini \
  && curl -fsSL https://github.com/krallin/tini/releases/download/${TINI_VERSION}/tini-static-${TINI_QUALIFIER}.asc -o /sbin/tini.asc \
  && gpg --keyserver hkp://p80.pool.sks-keyservers.net:80 --recv-keys 595E85A6B1B4779EA4DAAEC70B588DFF0527A9B7 \
  && gpg --verify /sbin/tini.asc \
  && rm -rf /sbin/tini.asc /root/.gnupg \
  && chmod +x /sbin/tini

ARG SWARM_CLIENT_VERSION=3.9
RUN set -x && \
    curl -sLo /usr/lib/jenkins/swarm-client.jar https://repo.jenkins-ci.org/releases/org/jenkins-ci/plugins/swarm-client/${SWARM_CLIENT_VERSION}/swarm-client-${SWARM_CLIENT_VERSION}.jar && \
    chmod 644 /usr/lib/jenkins/swarm-client.jar

RUN set -x && \
    curl -sLo /usr/lib/jenkins/password-encoder.jar https://github.com/openshift/jenkins/raw/f080514d4370c64260e2301e76f30061c5dcc636/2/contrib/openshift/password-encoder.jar && \
    chmod 644 /usr/lib/jenkins/password-encoder.jar

ENV JENKINS_VERSION=2 \
    HOME=/var/lib/jenkins \
    JENKINS_REF_HOME=/opt/jenkins \
    JENKINS_HOME=/var/lib/jenkins \
    JENKINS_UC=https://updates.jenkins.io \
    OPENSHIFT_JENKINS_IMAGE_VERSION=3.11 \
    LANG=en_US.UTF-8 \
    LC_ALL=en_US.UTF-8

#RUN set -x && \
#    curl -sLo /usr/local/bin/jenkins-install-plugins https://raw.githubusercontent.com/openshift/jenkins/master/2/contrib/jenkins/install-plugins.sh && \
#    mkdir -p ${JENKINS_REF_HOME} && \
#    chmod 555 /usr/local/bin/jenkins-install-plugins

COPY ./contrib/bin /usr/local/bin

RUN set -x && \
    java -version && \
    mkdir -p ${JENKINS_REF_HOME} && \
    mkdir -p $JENKINS_HOME && \
    chmod -R 666 $JENKINS_HOME && \
    chgrp -R 0 $JENKINS_HOME && \
    chmod -R g+rwX $JENKINS_HOME && \
    chmod 664 /etc/passwd && \
    chmod -R 666 /etc/sysconfig/jenkins && \
    chgrp -R 0 /var/log/jenkins && \
    chmod -R 666 /var/log/jenkins && \
    chgrp -R 0 /usr/local/bin && \
    chmod -R g+rx /usr/local/bin && \
    chgrp -R 0 /var/log && \
    chmod -R g+rwX /var/log && \
    chgrp -R 0 /var/cache/jenkins && \
    chmod -R g+rwX /var/cache/jenkins


LABEL io.k8s.description="Jenkins is a continuous integration server" \
      io.k8s.display-name="Jenkins 2" \
      io.openshift.tags="jenkins,jenkins2,ci" \
      io.openshift.expose-services="8080:http"

WORKDIR $HOME

USER 1001

